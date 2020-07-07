using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Company.CloudStorage;
using Company.Data;
using Company.Infrastructure;
using Company.Infrastructure.Filters;
using Company.Models;
using Company.Services;
using Company.Services.Publisher;
using Company.Services.Repository;
using Company.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Company.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class PreviewController : ControllerBase
    {
        private readonly ICloudStorage _cloudStorage;
        private readonly ICompanyPublisher _publisher;
        private readonly IRepository<Preview> _repository;

        public PreviewController(IRepository<Preview> repository, ICloudStorage cloudStorage,
            ICompanyPublisher publisher)
        {
            _cloudStorage = cloudStorage;
            _publisher = publisher;
            _repository = repository;
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create()
        {
            var companyId = Guid.NewGuid().ToString();
            var userId = HttpContext.UserId();

            await _repository.Add(new Preview
            {
                Id = companyId,
                OwnerId = userId,
                CreationDate = DateTime.UtcNow
            });

            await _publisher.PublishCreated(companyId, userId);

            return CreatedAtAction("Get", new {id = companyId}, new {id = companyId});
        }

        [HttpPatch]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(PreviewInputModel model)
        {
            var preview = await _repository.Find(model.Id);
            if (preview == null) return this.NotFoundResponse(model.Id);
            
            if (this.IsEntityCanBeChanged(preview, model.Id, out var result))
            {
                return result;
            }
            
            if (UpdatePreview(preview, model))
            {
                await _repository.Update(preview);
            }

            return NoContent();
        }

        [HttpPost]
        [Authorize]
        [FileValidator(20 * 1024 * 1024)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdatePreviewImage(string id, IFormFile previewImage)
        {
            if (previewImage == null) return Ok();

            var userId = HttpContext.UserId();
            var preview = await _repository.Find(id);
            if (this.IsEntityCanBeChanged(preview, id, out var result))
            {
                return result;
            }

            await DeleteImage(preview);

            var filename = _cloudStorage.CreateFileName(previewImage, userId);
            preview.ImageUrl = await _cloudStorage.UploadFileAsync(previewImage, filename);
            await _repository.Update(preview);

            return Ok(preview.ImageUrl);
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            var preview = await _repository.Find(id);

            if (this.IsEntityCanBeChanged(preview, id, out var result))
            {
                return result;
            }
            
            await DeleteImage(preview);
            await _repository.Delete(preview);
            await _publisher.PublishDeleted(preview.Id, HttpContext.UserId());

            return Ok();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(string id)
        {
            var entity = await _repository.Find(id);
            if (entity == null) return this.NotFoundResponse(id);

            return Ok(_repository.Mapper.Map<PreviewViewModel>(entity));
        }

        [HttpGet]
        [RangeFilter]
        [QuantileFilter]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRange(int take, int skip, int? quantile = default)
        {
            var expression = PreviewExpressionBuilder.Build(CompanyStatus.Lunched, quantile);
            var previews =
                await _repository.FindRange<PreviewViewModel, Funding>(take, skip, expression, i => i.Funding);

            return Ok(previews);
        }

        [HttpGet]
        [RangeFilter]
        [CategoryFilter]
        [QuantileFilter]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRangeByCategory(int take, int skip, int categoryId, int? quantile = default)
        {
            var expression = PreviewExpressionBuilder.Build(CompanyStatus.Lunched, quantile, categoryId);
            var previews =
                await _repository.FindRange<PreviewViewModel, Funding>(take, skip, expression, i => i.Funding);

            return Ok(previews);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserCompanies(int take, int skip, string userId = null)
        {
            List<CompanyProgress> previews;

            if (HttpContext.User.IsInRole(UserRoles.Admin) || HttpContext.UserId() == userId)
                previews = await _repository.FindRange<CompanyProgress>(take, skip, i => i.OwnerId == userId);
            else
                previews = await _repository.FindRange<CompanyProgress, Funding>(take, skip,
                    i => i.OwnerId == userId && i.Status == CompanyStatus.Lunched, i => i.Funding);

            return Ok(previews);
        }

        private async Task DeleteImage(Preview preview)
        {
            if (!string.IsNullOrEmpty(preview.ImageUrl))
            {
                await _cloudStorage.DeleteFileAsync(preview.ImageUrl);
            }
        }

        private bool UpdatePreview(Preview preview, PreviewInputModel model)
        {
            var previewModel = _repository.Mapper.Map<PreviewInputModel>(preview);
            if (previewModel.Equals(model))
            {
                return false;
            }
            
            preview.Title = model.Title;
            preview.Description = model.Description;
            preview.VideoUrl = model.VideoUrl;
            preview.CategoryId = model.CategoryId;
            preview.SubCategoryId = model.SubCategoryId;

            return true;
        }
    }
}