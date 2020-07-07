using Company.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Donation> Donations { get; set; }
        public DbSet<Preview> Previews { get; set; }
        public DbSet<Funding> Fundings { get; set; }
        public DbSet<Reward> Revards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Preview>()
                .HasOne(i => i.Funding)
                .WithOne()
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Funding>()
                .HasMany(i => i.Donations)
                .WithOne();

            modelBuilder.Entity<Reward>()
                .HasMany(i => i.Donations)
                .WithOne(i => i.Reward);

            modelBuilder.Entity<Reward>()
                .HasOne(i => i.Funding)
                .WithMany(i => i.Rewards);
        }
    }
}