namespace OrderMicroservice.DbContext;

using Microsoft.EntityFrameworkCore;
using Shared.Models;

public class OrderDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=order_db;User Id=postgres;Password=mypassword;");
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

