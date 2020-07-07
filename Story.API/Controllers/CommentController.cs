using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Story.API.Models;
using Story.API.Services.Interfaces;
using Updates.API.DatabaseSettings;

namespace Story.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class CommentController : ControllerBase
    {
        private readonly ILikeService<Comment> _likeService;
        private readonly IBaseRepository<Comment> _repository;
        private readonly IMongoCollection<CompanyStory> _updateCollection;

        public CommentController(
            IBaseRepository<Comment> repository,
            ILikeService<Comment> likeService,
            IDatabaseSettings settings)
        {
            _repository = repository;
            _likeService = likeService;

            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _updateCollection = database.GetCollection<CompanyStory>(nameof(CompanyStory));
        }

        [HttpGet]
        public async Task<IActionResult> Get(string commentId)
        {
            var comment = await _repository.GetAsync(commentId);

            if (comment == null) return NotFound($"Item with id: {commentId} not exist");

            return Ok(comment);
        }

        private FilterDefinition<CompanyStory> GetFilter(string id)
        {
            return Builders<CompanyStory>.Filter.Where(i => i.Id == id);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string newsId, string content)
        {
            var comment = new Comment
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                Content = content,
                LikesIds = new List<string>()
            };

            comment = await _repository.CreateAsync(comment);

            var updateNewsCount = Builders<CompanyStory>.Update.Inc(i => i.CommentsCount, 1);
            var updateNewsIds = Builders<CompanyStory>.Update.Push(i => i.CommentsIds, newsId);
            var update = Builders<CompanyStory>.Update.Combine(updateNewsCount, updateNewsIds);
            await _updateCollection.UpdateOneAsync(GetFilter(newsId), update);

            return CreatedAtAction("Get", new {comment.Id}, comment.Id);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string newsId, string id)
        {
            await _repository.RemoveAsync(id);

            var updateNewsCount = Builders<CompanyStory>.Update.Inc(i => i.CommentsCount, -1);
            var updateNewsIds = Builders<CompanyStory>.Update.Pull(i => i.CommentsIds, newsId);
            var update = Builders<CompanyStory>.Update.Combine(updateNewsCount, updateNewsIds);
            await _updateCollection.UpdateOneAsync(GetFilter(newsId), update);

            return Ok();
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> SetLike(string documentId)
        {
            var news = await _repository.GetAsync(documentId);

            if (news == null) return BadRequest($"Item with id: {documentId} does not exist");

            return Ok(await _likeService.SetLikeAsync(news, HttpContext.UserId(), _repository.GetCollection()));
        }
    }
}