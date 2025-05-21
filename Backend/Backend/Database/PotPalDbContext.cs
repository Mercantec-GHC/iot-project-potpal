using Microsoft.EntityFrameworkCore;
using Models;

namespace Database;

public class PotPalDbContext : DbContext
{
     public PotPalDbContext(DbContextOptions<PotPalDbContext> options) : base(options)
     { }

     public DbSet<User> Users { get; set; }
     public DbSet<Plant> Plants { get; set; }
     public DbSet<Metric> Metrics { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(u => new { u.Email })
            .IsUnique(true);

        modelBuilder.Entity<Metric>()
            .HasOne(m => m.Plant)
            .WithMany(p => p.Metrics)
            .HasForeignKey(m => m.PlantGUID)
            .HasPrincipalKey(p => p.GUID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Plant>()
            .HasOne(p => p.User)
            .WithMany(u => u.Plants)
            .HasForeignKey(p => p.UserEmail)
            .HasPrincipalKey(u => u.Email);
    }
}

