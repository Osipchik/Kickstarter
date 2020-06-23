using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EventBus.Events;
using Funding.API.Infrastructure;
using Funding.API.Models;
using Funding.API.Repositories;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Funding.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class DonationController : ControllerBase
    {
        private readonly Repository<Donation> _repository;
        private readonly IMapper _mapper;
        private readonly IBusControl _bus;
        
        public DonationController(ApplicationContext context, IMapper mapper, IBusControl bus)
        {
            _repository = new Repository<Donation>(context);
            _mapper = mapper;
            _bus = bus;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Donate(float amount, string companyId)
        {
            var donate = await _repository.Add(new Donation
            {
                UserId = HttpContext.UserId(),
                Amount = amount,
                FundingItemId = companyId,
                TransactionTime = DateTime.UtcNow
            });

            var uri = new Uri($"rabbitmq://localhost/Donate");
            var endpoint = await _bus.GetSendEndpoint(uri);
            await endpoint.Send<IDonate>(new {CompanyId = companyId, Amount = amount});
            
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> UserDonationHistory(string userId)
        {
            var donates = await _repository.FindByCondition(i => i.UserId == userId)
                .OrderBy(i => i.TransactionTime).ToListAsync();

            return Ok(donates);
        }
        
        [HttpGet]
        public async Task<IActionResult> CompanyDonationHistory(string companyId)
        {
            var donates = await _repository.FindByCondition(i => i.FundingItemId == companyId)
                .OrderBy(i => i.TransactionTime).ToListAsync();

            return Ok(donates);
        }
    }
}