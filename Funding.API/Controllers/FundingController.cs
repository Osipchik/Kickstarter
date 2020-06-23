using System;
using System.Threading.Tasks;
using AutoMapper;
using EventBus.Events;
using Funding.API.Infrastructure;
using Funding.API.Models;
using Funding.API.Repositories;
using Funding.API.ViewModels;
using MassTransit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Funding.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class FundingController : ControllerBase
    {
        private readonly Repository<FundingItem> _repository;
        private readonly IMapper _mapper;
        private readonly IBusControl _bus;
        
        public FundingController(ApplicationContext context, IMapper mapper, IBusControl bus)
        {
            _repository = new Repository<FundingItem>(context);
            _mapper = mapper;
            _bus = bus;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFunding(string id, bool auth)
        {
            if (auth)
            {
                return await GetAuthFundingItem(id);
            }
            
            var funding = await _repository.Find(id);

            if (funding?.StartFunding == null)
            {
                return NotFound();
            }
            
            return Ok(_mapper.Map<FundingViewModel>(funding));
        }
        
        private async Task<IActionResult> GetAuthFundingItem(string id)
        {
            var result = await HttpContext.AuthenticateAsync();
            if (!result.Succeeded)
            {
                return Unauthorized();
            }
            var funding = await _repository.Find(id);
            if (funding == null || funding.UserId != HttpContext.UserId())
            {
                return Forbid();
            }

            return Ok(_mapper.Map<FundingViewModel>(funding));
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> UpdateFunding([FromBody] FundingInputModel model)
        {
            if (!ModelIsValid(model))
            {
                return BadRequest();
            }
            
            var entity = await _repository.Find(model.Id);

            if (entity == null)
            {
                return NotFound();
            }

            if (model.EndDate != null)
            {
                model.EndDate = model.EndDate.Value.ToUniversalTime();
            }

            var intput = _mapper.Map<FundingInputModel>(entity);
            if (intput.Equals(model))
            {
                return Ok();
            }
            
            entity.Goal = model.Goal ?? 0;
            entity.EndFunding = model.EndDate;
            await _repository.Update(entity);
            await SendToBus(entity);
            
            return NoContent();
        }

        private async Task SendToBus(FundingItem entity)
        {
            var uri = new Uri($"rabbitmq://localhost/FundingUpdate");
            var endpoint = await _bus.GetSendEndpoint(uri);
            await endpoint.Send<IFundingUpdate>(new
            {
                CompanyId = entity.Id, 
                EndDate = entity.EndFunding,
                Goal = entity.Goal
            });
        }

        private bool ModelIsValid(FundingInputModel model)
        {
            var dateNow = DateTime.UtcNow;

            return model.Goal >= 10f || model.EndDate > dateNow || model.EndDate <= dateNow.AddMonths(2);
        }
    }
}