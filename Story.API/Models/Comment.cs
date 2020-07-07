using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Story.API.Services.Interfaces;

namespace Story.API.Models
{
    public class Comment : IDocument, ILikes
    {
        public string UserId { get; set; }

        [MaxLength(220)]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        public string Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public int LikesCount { get; set; }

        public List<string> LikesIds { get; set; }
    }
}