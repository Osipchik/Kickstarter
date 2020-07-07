using Company.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.API.Infrastructure
{
    public class ApplicationContext : DbContext
    {
        public DbSet<CompanyPreview> PreviewContext { get; set; }

        public ApplicationContext(DbContextOptions options) : base(options) { }
        
        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     modelBuilder.Entity<CompanyPreview>()
        //         .HasOne<CompanyStory>()
        //         .WithOne()
        //         .HasForeignKey<CompanyPreview>(i => i.CompanyStoryId)
        //         .OnDelete(DeleteBehavior.Cascade);
        // }
    }
}