using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Company.API.Infrastructure;
using Company.API.Models;
using EventBus.Events;
using MassTransit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Company.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class CompanyController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly IBusControl _bus;
        
        public CompanyController(ApplicationContext context, IBusControl bus)
        {
            _context = context;
            _bus = bus;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(string id, bool auth)
        {
            if (auth)
            {
                return await GetAuthCompanyItem(id);
            }
            
            return Ok(await _context.StoryContext.FindAsync(id));
        }
        
        private async Task<IActionResult> GetAuthCompanyItem(string id)
        {
            var result = await HttpContext.AuthenticateAsync();
            if (!result.Succeeded)
            {
                return Unauthorized();
            }
            var company = await _context.StoryContext.FindAsync(id);
            if (company == null || company.OwnerId != HttpContext.UserId())
            {
                return Forbid();
            }

            return Ok(company);
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateCompany()
        {
            var userId = HttpContext.UserId();
            var companyId = Guid.NewGuid().ToString();

            await _context.StoryContext.AddAsync(new CompanyStory
            {
                Id = companyId,
                OwnerId = userId,
                CreationDate = DateTime.Now
            });
            
            await _context.PreviewContext.AddAsync(new CompanyPreview
            {
                Id = companyId,
                OwnerId = userId,
                CompanyStoryId = companyId,
                Status = CompanyStatus.Draft
            });
            await _context.SaveChangesAsync();
            
            // await _bus.Publish<ICompanyCreated>(new
            // {
            //     CompanyId = companyId,
            //     UserId = userId
            // });
            
            await _bus.Publish<ICompanyEvent>(new
            {
                CompanyId = companyId,
                OwnerId = userId,
                Event = CompanyEvents.Created
            });
            
            return CreatedAtAction("Get", new {id = companyId}, new {id = companyId});
        }

        [HttpPost, Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateStory(string companyId, [FromBody]string story)
        {
            // TODO: get ownerId from accessToken

            var company = await _context.StoryContext.FindAsync(companyId);

            if (company == null)
            {
                return GetNotFoundResponse(companyId);
            }

            if (!HttpContext.User.IsInRole("admin") && company.OwnerId != HttpContext.UserId())
            {
                return Forbid();
            }

            company.Story = story;
            await Update(company);

            return NoContent();
        }
        
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateRisks(string companyId, [FromBody]string risks)
        {
            // TODO: get ownerId from accessToken

            var company = await _context.StoryContext.FindAsync(companyId);

            if (company == null)
            {
                return GetNotFoundResponse(companyId);
            }

            company.Risks = risks;
            await Update(company);

            return NoContent();
        }

        [HttpPost, Authorize(Roles = "user")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Launch(string companyId)
        {
            // TODO: get ownerId from accessToken
            
            var company = await _context.StoryContext.FindAsync(companyId);

            if (company == null)
            {
                return GetNotFoundResponse(companyId);
            }

            if (HttpContext.User.IsInRole("user") && company.OwnerId != HttpContext.UserId())
            {
                return Forbid();
            }

            company.LaunchDate = DateTime.Now.ToUniversalTime();
            await Update(company);
            
            // var uri = new Uri($"rabbitmq://localhost/CompanyLaunching");
            // var endpoint = await _bus.GetSendEndpoint(uri);
            // await endpoint.Send(new CompanyLaunching(companyId, DateTime.Now, 123));
            
            await _bus.Publish<ICompanyEvent>(new
            {
                CompanyId = companyId,
                UserId = HttpContext.UserId(),
                Event = CompanyEvents.Lunched
            });
            
            return CreatedAtAction("Get", new {id = companyId}, companyId);;
        }

        private NotFoundObjectResult GetNotFoundResponse(string id) =>
            NotFound(new {Message = $"Company with id: {id} not found."});
        
        private async Task Update(CompanyStory companyStory)
        {
            _context.StoryContext.Update(companyStory);
            await _context.SaveChangesAsync();
        }
    }
}