using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Company.API.CloudStorage;
using Company.API.Infrastructure;
using Company.API.Repositories;
using Company.API.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<UserPreviewViewModel>> GetUserPreviews(string userId)
        {
            var previews = _repository.FindAll().Where(i => i.OwnerId == userId);
            
            return await previews.Select(i => _mapper.Map<UserPreviewViewModel>(i)).ToListAsync();
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(string id, bool auth)
        {
            if (auth)
            {
                return await GetAuthPreviewItem(id);
            }
            
            var preview = await _repository.Find(id);
            if (preview == null)
            {
                return this.NotFoundResponse(id);
            }
            
            return Ok(_mapper.Map<PreviewViewModel>(preview));
        }
        
        private async Task<IActionResult> GetAuthPreviewItem(string id)
        {
            var result = await HttpContext.AuthenticateAsync();
            if (!result.Succeeded)
            {
                return Unauthorized();
            }
            var preview = await _repository.Find(id);
            if (preview == null || preview.OwnerId != HttpContext.UserId())
            {
                return Forbid();
            }

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

        [HttpPost, Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdatePreview([FromBody] PreviewInputModel model)
        {
            var preview = await _repository.FindUserPreview(model.Id, HttpContext.UserId());

            if (preview == null)
            {
                return this.NotFoundResponse(model.Id);
            }

            var previewModel = _mapper.Map<PreviewInputModel>(preview);

            if (previewModel.Equals(model))
            {
                return Ok();
            }

            preview.Title = model.Title;
            preview.Description = model.Description;
            preview.VideoUrl = model.VideoUrl;
            preview.CategoryId = model.CategoryId;
            preview.SubCategoryId = model.SubCategoryId;

            await _repository.Update(preview);
            
            return NoContent();
        }

        [HttpPost, Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdatePreviewImage(string previewId, IFormFile previewImage)
        {
            if (previewImage == null)
            {
                return Ok();
            }
            
            var userId = HttpContext.UserId();
            
            var preview = await _repository.FindUserPreview(previewId, userId);
            if (preview == null || !_cloudStorage.IsFileValid(previewImage))
            {
                return this.NotFoundResponse(previewId);
            }
            
            if (!string.IsNullOrEmpty(preview.ImageUrl))
            {
                await _cloudStorage.DeleteFileAsync(preview.ImageUrl);
            }
            
            var filename = _cloudStorage.CreateFileName(previewImage, userId);
            preview.ImageUrl = await _cloudStorage.UploadFileAsync(previewImage, filename);
            await _repository.Update(preview);
            
            return Ok(preview.ImageUrl);
        }

        [HttpDelete, Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteImage(string previewId)
        {
            var preview = await _repository.Find(previewId);
            if (preview == null)
            {
                return this.NotFoundResponse(previewId);
            }

            await _cloudStorage.DeleteFileAsync(preview.ImageUrl);
            preview.ImageUrl = null;
            await _repository.Update(preview);
            
            return Ok();
        }
    }
}
