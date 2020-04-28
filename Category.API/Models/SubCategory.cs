namespace Category.API.Models
{
    public class SubCategory
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        
        public string SubCategoryName { get; set; }
    }
}