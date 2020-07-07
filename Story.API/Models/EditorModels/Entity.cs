using System.Collections.Generic;

namespace Story.API.Models.EditorModels
{
    public class Entity
    {
        public string Type { get; set; }
        public string Mutability { get; set; }
        public Dictionary<string, string> Data { get; set; } = new Dictionary<string, string>();
    }
}