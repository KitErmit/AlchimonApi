using System;
using AlchimonAng.Models;
using Microsoft.EntityFrameworkCore;

namespace AlchimonAng.DB
{
    public class AlBdContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Alchemon> Alchemons { get; set; }
        public AlBdContext()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseNpgsql("Host=localhost;Port=5432;Database=albase;Username=postgres;Password=1")
            .LogTo(System.Console.WriteLine, LogLevel.Error);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Player>()
                .HasIndex(p => p.Id).IsUnique();
            
            modelBuilder.Entity<Alchemon>().HasIndex(d => d.Id).IsUnique();
            modelBuilder.Entity<Player>().HasMany(p => p.Karman)
                .WithOne(a => a.Player)
                .HasForeignKey(a => a.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
            
        }
    }
}
