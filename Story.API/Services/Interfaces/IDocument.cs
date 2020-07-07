using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Story.API.Services.Interfaces
{
    public interface IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        string Id { get; set; }

        DateTime CreatedAt { get; set; }
    }
}