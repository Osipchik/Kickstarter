using System;
using System.ComponentModel.DataAnnotations;

namespace Company.ViewModels
{
    public class PreviewInputModel : IEquatable<PreviewInputModel>
    {
        [Required] public string Id { get; set; }

        [StringLength(60)] public string Title { get; set; }

        [StringLength(135)] public string Description { get; set; }

        [DataType(DataType.Url)] public string VideoUrl { get; set; }

        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }

        public bool Equals(PreviewInputModel other)
        {
            if (other == null)
                return false;

            return Title == other.Title &&
                   Description == other.Description &&
                   VideoUrl == other.VideoUrl &&
                   CategoryId == other.CategoryId &&
                   SubCategoryId == other.SubCategoryId;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            return ReferenceEquals(this, obj) || Equals(obj as PreviewInputModel);
        }
    }
}