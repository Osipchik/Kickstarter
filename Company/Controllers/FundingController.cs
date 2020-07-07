using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Company.Infrastructure;
using Company.Infrastructure.Filters;
using Company.Models;
using Company.Services.Repository;
using Company.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class FundingController : ControllerBase
    {
        private readonly IRepository<Donation> _donationRepository;
        // private readonly FundingRepository _fundingRepository;
        // private readonly DonationRepository _donationRepository;
        // private readonly RewardRepository _rewardRepository;

        private readonly IRepository<Funding> _fundingRepository;
        private readonly IRepository<Reward> _rewardRepository;

        public FundingController(
            IRepository<Funding> fundingRepository,
            IRepository<Donation> donationRepository,
            IRepository<Reward> rewardRepository)
        {
            _fundingRepository = fundingRepository;
            _donationRepository = donationRepository;
            _rewardRepository = rewardRepository;
        }

        [HttpPatch]
        [Authorize]
        [DonateFilter]
        public async Task<IActionResult> Donate(string id, float amount)
        {
            var reward = await _rewardRepository.Find(id);
            if (reward == null || reward.DonationCount.HasValue && reward.DonationCount.Value == 0) return BadRequest();

            var donation = new Donation
            {
                Id = Guid.NewGuid().ToString(),
                Amount = amount,
                FundingId = reward.FundingId,
                RewardId = reward.Id,
                OwnerId = HttpContext.UserId(),
                TransactionTime = DateTime.UtcNow
            };

            reward.Funding.Funded += amount;

            await _donationRepository.Add(donation);
            // await _fundingRepository.Update(funding);

            reward = await _rewardRepository.Find(id);

            return Ok();
        }

        [HttpGet]
        [Authorize]
        [RangeFilter]
        public async Task<IActionResult> CompanyDonationHistory(int take, int skip, string fundingId)
        {
            var donations = HttpContext.User.IsInRole(UserRoles.Admin)
                ? await _donationRepository.FindRange<DonationViewModel>(take, skip, i => i.FundingId == fundingId)
                : await _donationRepository.FindRange<DonationViewModel>(take, skip,
                    i => i.FundingId == fundingId && i.OwnerId == HttpContext.UserId());
            return Ok(donations);
        }

        [HttpGet]
        [Authorize]
        [RangeFilter]
        public async Task<IActionResult> UserDonationHistory(int take, int skip, string userId = null)
        {
            List<DonationViewModel> donations;
            if (HttpContext.User.IsInRole(UserRoles.Admin))
            {
                donations = userId == null
                    ? await _donationRepository.FindRange<DonationViewModel>(take, skip,
                        i => i.OwnerId == HttpContext.UserId())
                    : await _donationRepository.FindRange<DonationViewModel>(take, skip, i => i.OwnerId == userId);
            }
            else
            {
                userId = HttpContext.UserId();
                donations = await _donationRepository.FindRange<DonationViewModel>(take, skip,
                    i => i.OwnerId == userId);
            }

            return Ok(donations);
        }
    }
}