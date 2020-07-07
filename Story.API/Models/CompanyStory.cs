using System;
using System.Collections.Generic;
using Story.API.Models.EditorModels;
using Story.API.Services.Interfaces;

namespace Story.API.Models
{
    public class CompanyStory : IDocument, ILikes
    {
        public bool IsAccepted { get; set; }

        public string Title { get; set; }

        public List<Block> Blocks { get; set; }

        public Dictionary<string, Entity> EntityMap { get; set; }


        public int CommentsCount { get; set; }

        public List<string> CommentsIds { get; set; }
        public string Id { get; set; }

        public DateTime CreatedAt { get; set; }


        public int LikesCount { get; set; }

        public List<string> LikesIds { get; set; }
    }
}