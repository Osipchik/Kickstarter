using System.Collections.Generic;

namespace Category.API.Models
{
    public class Category
    {
        public int Id { get; set; }
        
        public string CategoryName { get; set; }
        public ICollection<SubCategory> SubCategories { get; set; }
    }
}