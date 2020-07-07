using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using Story.API.Services.Interfaces;

namespace Story.API.Models
{
    public class Company : IDocument
    {
        public int StoriesCount { get; set; }

        public List<string> StoriesIds { get; set; }

        public bool IsLunched { get; set; }

        [BsonId] public string Id { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}