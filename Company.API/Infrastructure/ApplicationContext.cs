using Company.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.API.Infrastructure
{
    public class ApplicationContext : DbContext
    {
        public DbSet<CompanyItem> CompanyContext { get; set; }
        public DbSet<CompanyPreview> PreviewContext { get; set; }
        public DbSet<CompanyFunding> FundingContext { get; set; }
        // public DbSet<CompanyReward> RewardContext { get; set; }

        public ApplicationContext(DbContextOptions options) : base(options) { }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompanyPreview>()
                .HasOne(i => i.CompanyItem)
                .WithOne(p => p.CompanyPreview)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CompanyPreview>()
                .HasOne(i => i.CompanyFunding)
                .WithOne(p => p.CompanyPreview)
                .OnDelete(DeleteBehavior.Cascade);

            
            modelBuilder.Entity<CompanyFunding>()
                .HasOne(p => p.CompanyPreview)
                .WithOne(f => f.CompanyFunding)
                .HasForeignKey<CompanyPreview>(i => i.CompanyFundingId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CompanyItem>()
                .HasMany(i => i.CompanyImages)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            // modelBuilder.Entity<CompanyReward>()
            //     .HasOne<CompanyItem>()
            //     .WithMany()
            //     .HasForeignKey(p => p.CompanyId)
            //     .OnDelete(DeleteBehavior.Cascade);
        }
    }
}