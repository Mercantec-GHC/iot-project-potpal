using Microsoft.EntityFrameworkCore;
using Models;

namespace Database;

public class PotPalDbContext : DbContext
{
    public PotPalDbContext(DbContextOptions<PotPalDbContext> options) : base(options)
    { }

    public DbSet<User> Users { get; set; }
    public DbSet<Plant> Plants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
        .HasIndex(u => new { u.Email })
        .IsUnique(true);
    }
}

