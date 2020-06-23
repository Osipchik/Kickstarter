using Funding.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Funding.API.Infrastructure
{
    public class ApplicationContext : DbContext
    {
        public DbSet<FundingItem> FundingContext { get; set; }
        public DbSet<Donation> DonationsContext { get; set; }
        public DbSet<Reward> RewardsContext { get; set; }
        
        public ApplicationContext(DbContextOptions options) : base(options) { }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Donation>()
                .HasOne<FundingItem>()
                .WithMany()
                .HasForeignKey(i => i.FundingItemId);
        }
    }
}