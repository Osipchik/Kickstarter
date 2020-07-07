using System.Collections.Generic;

namespace Story.API.Models.EditorModels
{
    public class Block
    {
        public string Key { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }
        public int Depth { get; set; }
        public List<InlineStyle> InlineStyleRanges { get; set; } = new List<InlineStyle>();
        public List<BlockEntity> EntityRanges { get; set; } = new List<BlockEntity>();
        public Dictionary<string, string> Data { get; set; } = new Dictionary<string, string>();
    }
}