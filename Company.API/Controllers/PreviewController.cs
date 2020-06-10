using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Company.API.CloudStorage;
using Company.API.Repositories;
using Company.API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class PreviewController : ControllerBase
    {
        private readonly PreviewRepository _repository;
        private readonly ICloudStorage _cloudStorage;
        private readonly IMapper _mapper;

        public PreviewController(PreviewRepository repository, IMapper mapper, ICloudStorage cloudStorage)
        {
            _cloudStorage = cloudStorage;
            _repository = repository;
            _mapper = mapper;
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<PreviewViewModel>> GetPreviews(int take, int skip = 0)
        {
            var previews = _repository.FindAll().Skip(skip).Take(take);
            
            return await previews.Select(i => _mapper.Map<PreviewViewModel>(i)).ToListAsync();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(string id)
        {
            var preview = await _repository.Find(id);
        
            if (preview == null)
            {
                return NotFound();
            }

            await _repository.LoadFundingAsync(preview);
            
            return Ok(_mapper.Map<PreviewViewModel>(preview));
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<PreviewViewModel>> GetByCategory(int take, int categoryId, int skip = 0, int subCategoryId = 0)
        {
            var previews = _repository.FindByCategory(categoryId, subCategoryId)
                .Skip(skip).Take(take)
                .Select(i => _mapper.Map<PreviewViewModel>(i));
            
            return await previews.ToListAsync();
        }
        
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateDescription(string previewId, string description)
        {
            // TODO: get ownerId from accessToken
        
            var preview = await _repository.Find(previewId);
            if (preview == null)
            {
                return NotFound(new {Message = $"Item with id: {previewId} not found."});
            }
        
            preview.Description = description;

            await _repository.Update(preview);

            return Ok();
        }

        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateTitle(string previewId, string title)
        {
            // TODO: get ownerId from accessToken
        
            var preview = await _repository.Find(previewId);
            if (preview == null)
            {
                return NotFound(new {Message = $"Item with id: {previewId} not found."});
            }
        
            preview.Title = title;

            await _repository.Update(preview);

            return Ok();
        }
        
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateCategory(string previewId, int categoryId, int? subCategoryId)
        {
            // TODO: get ownerId from accessToken
           
            if (categoryId < 0)
            {
                return BadRequest();
            }
        
            var preview = await _repository.Find(previewId);
            if (preview == null)
            {
                return NotFound(new {Message = $"Item with id: {previewId} not found."});
            }
        
            preview.CategoryId = categoryId;
            if (subCategoryId.HasValue && subCategoryId.Value > 0)
            {
                preview.SubCategoryId = subCategoryId.Value;
            }

            await _repository.Update(preview);

            return Ok();
        }
        
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateVideoUrl(string previewId, string videoUrl)
        {
            // TODO: get ownerId from accessToken
            
            var preview = await _repository.Find(previewId);
            if (preview == null)
            {
                return NotFound(new {Message = $"Item with id: {previewId} not found."});
            }

            preview.VideoUrl = videoUrl;
            await _repository.Update(preview);
            
            return Ok();
        }
        
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdatePreviewImage(string previewId, IFormFile previewImage)
        {
            // TODO: get ownerId from accessToken
            
            var preview = await _repository.Find(previewId);
            if (preview == null || !_cloudStorage.IsFileValid(previewImage))
            {
                return BadRequest();
            }

            if (!string.IsNullOrEmpty(preview.ImageUrl))
            {
                await _cloudStorage.DeleteFileAsync(preview.ImageUrl);
            }
            
            var filename = _cloudStorage.CreateFileName(previewImage, "1");
            preview.ImageUrl = await _cloudStorage.UploadFileAsync(previewImage, filename);
            await _repository.Update(preview);
            
            return Ok(preview.ImageUrl);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteImage(string previewId)
        {
            var preview = await _repository.Find(previewId);
            if (preview == null)
            {
                return NotFound();
            }

            await _cloudStorage.DeleteFileAsync(preview.ImageUrl);
            preview.ImageUrl = null;
            await _repository.Update(preview);
            
            return Ok();
        }
    }
}
