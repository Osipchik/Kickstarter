using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MassTransit.Initializers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Story.API.Models;
using Story.API.Models.EditorModels;
using Story.API.Services;
using Story.API.Services.Interfaces;
using Story.API.ViewModels;

namespace Story.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class StoryController : ControllerBase
    {
        private readonly CompanyRepository _companyRepository;
        private readonly ILikeService<CompanyStory> _likeService;
        private readonly StoryRepository _storyRepository;

        public StoryController(
            StoryRepository storyRepository,
            ILikeService<CompanyStory> likeService,
            CompanyRepository companyRepository)
        {
            _storyRepository = storyRepository;
            _likeService = likeService;
            _companyRepository = companyRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string id)
        {
            // var story = await _repository.GetAsync(id);
            //
            // if (story == null)
            // {
            //     return NotFound($"Item with id: {id} not found");
            // }

            var filter = Builders<CompanyStory>.Filter.Eq(i => i.Id, id);
            var asd = await _storyRepository.GetAsync(id);
            var story = await _storyRepository.GetCollection().Find(filter).FirstOrDefaultAsync().Select(i => new
            {
                i.Id,
                i.CreatedAt,
                i.LikesCount,
                i.CommentsCount,
                i.Blocks,
                i.EntityMap,
                isLiked = i.LikesIds.Contains(HttpContext.UserId())
            });

            return Ok(story);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string companyId, EditorInputModel editorModel)
        {
            if (!_companyRepository.IsDocumentExist(companyId))
                return NotFound($"Company with id: {companyId} not found");

            // await UploadImages(editorModel.EntityMap, files);

            var storyId = Guid.NewGuid().ToString();
            var story = new CompanyStory
            {
                Id = storyId,
                CreatedAt = DateTime.UtcNow,
                Blocks = editorModel.Blocks,
                EntityMap = editorModel.EntityMap,
                LikesIds = new List<string>(),
                CommentsIds = new List<string>()
            };
            await _storyRepository.CreateAsync(story);
            await _companyRepository.AddStory(companyId, storyId);

            return CreatedAtAction("Get", new {Id = storyId}, storyId);
        }

        [HttpPatch]
        public async Task<IActionResult> Update(string storyId, EditorInputModel editorModel, List<IFormFile> files)
        {
            if (!_storyRepository.IsDocumentExist(storyId)) return NotFound($"Story with id: {storyId} not found");

            await UploadImages(editorModel.EntityMap, files);

            await _storyRepository.Update(storyId, editorModel, files);

            return NoContent();
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete(string companyId, string id)
        {
            await _storyRepository.RemoveAsync(id);

            await _companyRepository.RemoveStory(companyId, id);

            return Ok();
        }

        private async Task UploadImages(Dictionary<string, Entity> entityMap, List<IFormFile> images)
        {
            var count = 0;
            const string url = "url";
            foreach (var (key, value) in entityMap)
                if (value.Type == "image" && value.Data[url].Contains("."))
                    // entityMap[key].Data[url] = await Upload(value.Data[url]);
                    value.Data[url] = await Upload(value.Data[url]);
        }

        private async ValueTask<string> Upload(string name)
        {
            return Guid.NewGuid().ToString();
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> SetLike(string documentId)
        {
            var story = await _storyRepository.GetAsync(documentId);

            if (story == null) return BadRequest($"Item with id: {documentId} does not exist");

            return Ok(await _likeService.SetLikeAsync(story, HttpContext.UserId(), _storyRepository.GetCollection()));
        }
    }
}