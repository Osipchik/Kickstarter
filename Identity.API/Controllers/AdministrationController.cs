using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class AdministrationController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleManager.Roles.AsNoTracking().ToListAsync();

            return Ok(roles);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            var role = new IdentityRole{ Name = roleName };

            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                return CreatedAtAction("CreateRole", new {roleName = roleName}, null);
            }
            
            return GetRoleErrorsRequest(result, null);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateRole(string roleId, string roleName)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                return NotFound($"Item with id: {roleId} not exist");
            }

            role.Name = roleName;
            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                return NoContent();
            }
            
            return GetRoleErrorsRequest(result, roleId);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EditUsersInRole(List<string> usersIds, string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return NotFound($"Item with id: {roleId} not exist");
            }

            foreach (var i in usersIds)
            {
                var user = await _userManager.FindByIdAsync(i);
                var isInRole = await _userManager.IsInRoleAsync(user, role.Name);

                var result = isInRole
                    ? await _userManager.RemoveFromRoleAsync(user, role.Name)
                    : await _userManager.AddToRoleAsync(user, role.Name);
                
                if (result != null && !result.Succeeded)
                {
                    return GetRoleErrorsRequest(result, roleId);
                }
            }
            
            return Ok();
        }

        private BadRequestObjectResult GetRoleErrorsRequest(IdentityResult result, string roleId)
        {
            var errors = result.Errors.Select(i => i.Description).ToList();
            
            return BadRequest(new {RoleId = roleId, Errors = errors});
        }
    }
}