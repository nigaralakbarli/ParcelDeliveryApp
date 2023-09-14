namespace DeliveryMicroservice.DbContext;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

public class DeliveryDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=OrderDb;Username=postgres;Password=nigaR123");
    }

    public DbSet<Order> Orders => Set<Order>();

}
