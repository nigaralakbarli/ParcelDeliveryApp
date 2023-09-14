namespace OrderMicroservice.DbContext;

using Microsoft.EntityFrameworkCore;
using Shared.Models;

public class OrderDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=OrderDb;Username=postgres;Password=nigaR123");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>()
            .Property(a => a.OrderStatus)
            .HasConversion<string>();

        base.OnModelCreating(modelBuilder);
    }
    public DbSet<Order> Orders => Set<Order>();
}
