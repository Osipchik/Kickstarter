using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Company.API.Infrastructure;
using Company.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class PreviewController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public PreviewController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<CompanyPreview>> GetPreviews(int take, int skip = 0)
        {
            var previews = _context.PreviewContext.AsNoTracking()
                .Skip(skip).Take(take);

            return await previews.ToListAsync();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(string id)
        {
            var preview = await _context.PreviewContext.FindAsync(id);

            if (preview == null)
            {
                return NotFound();
            }

            return Ok(preview);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<CompanyPreview>> GetByCategory(int take, int categoryId, int skip = 0, int subCategoryId = 0)
        {
            var previews = _context.PreviewContext.AsNoTracking();

            previews = subCategoryId > 0
                ? previews.Where(i => i.CategoryId == categoryId && i.SubCategoryId == subCategoryId)
                : previews.Where(i => i.CategoryId == categoryId);

            return await previews.Skip(skip).Take(take).ToListAsync();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> UpdatePreview([FromBody]CompanyPreview companyPreviewToUpdate)
        {
            // TODO: get ownerId from accessToken
            
            if (await _context.PreviewContext.AnyAsync(i => i.Id == companyPreviewToUpdate.Id))
            {
                return NotFound(new {Message = $"Item with id: {companyPreviewToUpdate.Id} not found."});
            }
        
            _context.PreviewContext.Update(companyPreviewToUpdate);
            await _context.SaveChangesAsync();
        
            return CreatedAtAction(nameof(GetById), new {id = companyPreviewToUpdate.Id}, null);
        }
        
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateDescription(string previewId, string description)
        {
            // TODO: get ownerId from accessToken

            var preview = await _context.PreviewContext.FindAsync(previewId);
            if (preview == null)
            {
                return NotFound(new {Message = $"Item with id: {previewId} not found."});
            }

            preview.Description = description;

            _context.PreviewContext.Update(preview);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}