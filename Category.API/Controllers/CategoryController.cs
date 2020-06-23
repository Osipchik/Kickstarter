using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Category.API.Data;
using Category.API.Models;
using EventBus;
using EventBus.Events;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Category.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class CategoryController : ControllerBase
    {
        private readonly IBusControl _bus;
        private readonly ApplicationContext _context;
        
        public CategoryController(ApplicationContext context, IBusControl bus)
        {
            _bus = bus;
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<Models.Category>> GetCategories()
        {
            var categories = _context.CategoryContext.AsNoTracking()
                .Include(s => s.SubCategories);

            return await categories.ToListAsync();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSubCategories(int categoryId)
        {
            var categories = _context.SubCategoryContext.AsNoTracking()
                .Where(i => i.CategoryId == categoryId)
                .Select(i => new { id = i.Id, name = i.SubCategoryName });
            
            return Ok(await categories.ToListAsync());
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> AddCategory(string categoryName)
        {
            // TODO: get ownerId from accessToken
            
            if (await _context.CategoryContext.AsNoTracking().AnyAsync(n => n.CategoryName == categoryName))
            {
                return GetAlreadyExistResponse;
            }
            
            await Add(new Models.Category { CategoryName = categoryName});
            
            return CreatedAtAction(nameof(GetCategories), null, null);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> AddSubCategory(int categoryId, string subCategoryName)
        {
            // TODO: get ownerId from accessToken
            
            if (await _context.SubCategoryContext.AsNoTracking().AnyAsync(n => n.SubCategoryName == subCategoryName))
            {
                return GetAlreadyExistResponse;
            }

            if (!await _context.CategoryContext.AsNoTracking().AnyAsync(i => i.Id == categoryId))
            {
                return GetNotFoundResponse(categoryId);
            }

            var subCategory = new SubCategory
            {
                SubCategoryName = subCategoryName,
                CategoryId = categoryId
            };

            await Add(subCategory);
            
            return CreatedAtAction(nameof(GetSubCategories), new {categoryId = subCategory.CategoryId}, null);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteCategory(int id, int newId)
        {
            // TODO: get ownerId from accessToken
            
            var category = await _context.CategoryContext.FindAsync(id);

            if (category == null)
            {
                return GetNotFoundResponse(id);
            }

            if (await _context.CategoryContext.CountAsync() == 1)
            {
                return BadRequest("You cannot delete the last category. At least one category must exist.");
            }
            
            _context.CategoryContext.Remove(category);
            await _context.SaveChangesAsync();
            
            var uri = new Uri($"rabbitmq://localhost/categoryChanges");
            var endpoint = await _bus.GetSendEndpoint(uri);
            await endpoint.Send(new CategoryDeletingMessage(category.Id, newId));
            
            return NoContent();
        }
        
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteSubCategory(int id)
        {
            // TODO: get ownerId from accessToken
            
            var category = await _context.SubCategoryContext.FindAsync(id);

            if (category == null)
            {
                return GetNotFoundResponse(id);
            }

            _context.SubCategoryContext.Remove(category);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
        
        private async Task Add<TEntity>(TEntity entity) where TEntity : class
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        
        private BadRequestObjectResult GetAlreadyExistResponse => BadRequest("This category is already exist");
        private NotFoundObjectResult GetNotFoundResponse(int id) => NotFound($"Category with id: {id} not found");
    }
}