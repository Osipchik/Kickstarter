using Company.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.API.Infrastructure
{
    public class ApplicationContext : DbContext
    {
        public DbSet<CompanyItem> CompanyContext { get; set; }
        public DbSet<CompanyPreview> PreviewContext { get; set; }
        // public DbSet<CompanyReward> RewardContext { get; set; }

        public ApplicationContext(DbContextOptions options) : base(options) { }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompanyPreview>()
                .HasOne<CompanyItem>()
                .WithOne()
                .HasForeignKey<CompanyPreview>(p => p.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CompanyReward>()
                .HasOne<CompanyItem>()
                .WithMany()
                .HasForeignKey(p => p.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}