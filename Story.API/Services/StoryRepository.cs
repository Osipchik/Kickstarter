using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using Story.API.Models;
using Story.API.ViewModels;
using Updates.API.DatabaseSettings;

namespace Story.API.Services
{
    public class StoryRepository : BaseRepository<CompanyStory>
    {
        public StoryRepository(IDatabaseSettings settings) : base(settings)
        {
        }

        public async Task Update(string storyId, EditorInputModel editorModel, List<IFormFile> files)
        {
            var filter = Builders<CompanyStory>.Filter.Eq(i => i.Id, storyId);
            var updateBlocks = Builders<CompanyStory>.Update.Set(i => i.Blocks, editorModel.Blocks);
            var updateEntityMap = Builders<CompanyStory>.Update.Set(i => i.EntityMap, editorModel.EntityMap);
            var update = Builders<CompanyStory>.Update.Combine(updateBlocks, updateEntityMap);
            await _collection.UpdateOneAsync(filter, update);
        }
    }
}