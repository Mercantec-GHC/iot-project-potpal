using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Models;

namespace Database;

public class PotPalDbContext : DbContext
{
    public PotPalDbContext(DbContextOptions<PotPalDbContext> options) : base(options)
    { }

    public DbSet<User> Users { get; set; }
    public DbSet<Plant> Plants { get; set; }
    public DbSet<Metric> Metrics { get; set; }
    public DbSet<ShopItem> ShopItems { get; set; }
    public DbSet<CartItem> CartItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
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

        modelBuilder.Entity<CartItem>()
            .HasKey(ci => new { ci.UserToken, ci.ItemId });

        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.User)
            .WithMany(u => u.CartItems)
            .HasForeignKey(ci => ci.UserToken);

        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.ShopItem)
            .WithMany(si => si.CartItems)
            .HasForeignKey(ci => ci.ItemId);
    }
}

