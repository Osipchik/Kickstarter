using System;
using System.Threading.Tasks;
using Company.API.Infrastructure;
using Company.API.Models;
using MassTransit;
using Messages;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<CompanyItem> Get(string id)
        {
            return await _context.CompanyContext.FindAsync(id);
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateCompany(string ownerId)
        {
            // TODO: get ownerId from accessToken

            var companyId = Guid.NewGuid().ToString();
            await _context.CompanyContext.AddAsync(new CompanyItem
            {
                Id = companyId,
                OwnerId = ownerId,
                CreationDate = DateTime.Now
            });
            
            await _context.PreviewContext.AddAsync(new CompanyPreview {Id = Guid.NewGuid().ToString()});
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new {id = companyId}, null);
        }

        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateStory(string companyId, [FromBody]string story)
        {
            // TODO: get ownerId from accessToken

            var company = await _context.CompanyContext.FindAsync(companyId);

            if (company == null)
            {
                return GetNotFoundResponse(companyId);
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

            var company = await _context.CompanyContext.FindAsync(companyId);

            if (company == null)
            {
                return GetNotFoundResponse(companyId);
            }

            company.Risks = risks;
            await Update(company);

            return NoContent();
        }

        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Launch(string companyId)
        {
            // TODO: get ownerId from accessToken
            
            var company = await _context.CompanyContext.FindAsync(companyId);

            if (company == null)
            {
                return GetNotFoundResponse(companyId);
            }

            company.LaunchDate = DateTime.Now.ToUniversalTime();
            await Update(company);
            
            var uri = new Uri($"rabbitmq://localhost/CompanyLaunching");
            var endpoint = await _bus.GetSendEndpoint(uri);
            await endpoint.Send(new CompanyLaunching(companyId, DateTime.Now, 123));
            
            return CreatedAtAction(nameof(Get), new {id = companyId}, null);;
        }

        private NotFoundObjectResult GetNotFoundResponse(string id) =>
            NotFound(new {Message = $"Company with id: {id} not found."});
        
        private async Task Update(CompanyItem companyItem)
        {
            _context.CompanyContext.Update(companyItem);
            await _context.SaveChangesAsync();
        }
    }
}