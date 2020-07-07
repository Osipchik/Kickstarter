using System.Collections.Generic;

namespace Story.API.Services.Interfaces
{
    public interface ILikes
    {
        public int LikesCount { get; set; }

        public List<string> LikesIds { get; set; }
    }
}