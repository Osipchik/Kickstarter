using System.Collections.Generic;
using Story.API.Models.EditorModels;

namespace Story.API.ViewModels
{
    public class EditorInputModel
    {
        public List<Block> Blocks { get; set; }
        public Dictionary<string, Entity> EntityMap { get; set; }
    }
}