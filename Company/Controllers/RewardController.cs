using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Company.Infrastructure;
using Company.Infrastructure.Filters;
using Company.Models;
using Company.Models.EditorModels;
using Company.Services.Repository;
using Company.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Company.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class RewardController : ControllerBase
    {
        private readonly IRepository<Funding> _fundingRepository;
        private readonly IRepository<Reward> _rewardRepository;

        public RewardController(IRepository<Reward> rewardRepository, IRepository<Funding> fundingRepository)
        {
            _rewardRepository = rewardRepository;
            _fundingRepository = fundingRepository;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(string fundingId, int? amount, [FromBody]List<Block> content)
        {
            var funding = await _fundingRepository.Find(fundingId, i => i.Rewards);

            if (this.IsEntityCanBeChanged(funding, fundingId, out var result))
            {
                return result;
            }

            if (funding.Rewards.Count() >= 10)
            {
                return BadRequest();
            }
            
            var reward = new Reward
            {
                Id = Guid.NewGuid().ToString(),
                DonationCount = amount,
                FundingId = fundingId,
                // TODO:
            };

            await _rewardRepository.Add(reward);

            return CreatedAtAction("Get", new {Id = reward.Id}, reward.Id);
        }
        
        [HttpPatch]
        [Authorize]
        public async Task<IActionResult> Update(string id, int? amount, [FromBody]List<Block> content)
        {
            var reward = await _rewardRepository.Find(id);

            if (this.IsEntityCanBeChanged(reward, id, out var result))
            {
                return result;
            }

            reward.DonationCount = amount;
            // TODO:

            await _rewardRepository.Update(reward);

            return Ok();
        }
        
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            var reward = await _rewardRepository.Find(id);

            if (this.IsEntityCanBeChanged(reward, id, out var result))
            {
                return result;
            }

            await _rewardRepository.Delete(reward);

            return Ok();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(string id)
        {
            var entity = await _rewardRepository.Find(id);
            if (entity == null) return this.NotFoundResponse(id);

            return Ok(_rewardRepository.Mapper.Map<RewardViewModel>(entity));
        }

        [HttpGet]
        [RangeFilter]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRange(int take, int skip, string findingId)
        {
            var rewards = await _rewardRepository.FindRange<RewardViewModel>(take, skip, i => i.FundingId == findingId);

            return Ok(rewards);
        }
    }
}