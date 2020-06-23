using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funding.API.Infrastructure;
using Funding.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Funding.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class RewardsController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private const int MaxRewardsCount = 10;
        
        public RewardsController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<Reward>> GetRewards(string companyId)
        {
            var rewards = await _context.RewardsContext.AsNoTracking()
                .Where(i => i.CompanyId == companyId)
                .ToListAsync();

            return rewards;
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Add(string companyId, string content, int amount)
        {
            // TODO: get user id
            
            var rewardsCount = await _context.RewardsContext.AsNoTracking()
                .Where(i => i.CompanyId == companyId)
                .CountAsync();

            if (rewardsCount == MaxRewardsCount)
            {
                return BadRequest($"You cant have more then {MaxRewardsCount} rewards");
            }
            
            var reward = new Reward
            {
                CompanyId = companyId,
                Content = content,
                Amount = amount
            };

            await _context.RewardsContext.AddAsync(reward);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetRewards), new {CompanyId = companyId}, null);
        }
        
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> UpdateReward(string rewardId, string content, int amount)
        {
            // TODO: get user id
            
            var reward = await _context.RewardsContext.FindAsync(rewardId);

            if (reward == null)
            {
                return NotFound($"Item with id {rewardId} not found");
            }

            reward.Content = content;
            reward.Amount = amount;
            _context.RewardsContext.Update(reward);
            await _context.SaveChangesAsync();
            
            return Ok();
        }
        
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteReward(string rewardId)
        {
            // TODO: get user id
            
            var reward = await _context.RewardsContext.FindAsync(rewardId);

            if (reward == null)
            {
                return NotFound($"Item with id {rewardId} not found");
            }

            _context.RewardsContext.Remove(reward);
            await _context.SaveChangesAsync();
            
            return Ok();
        }
    }
}