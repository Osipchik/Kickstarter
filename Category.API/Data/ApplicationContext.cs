using Category.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Category.API.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Models.Category> CategoryContext { get; set; }
        public DbSet<SubCategory> SubCategoryContext { get; set; }
        
        public ApplicationContext(DbContextOptions options) : base(options) { }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Category>()
                .HasMany(s => s.SubCategories)
                .WithOne()
                .HasForeignKey(i => i.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
            
            base.OnModelCreating(modelBuilder);
        }
    }
}