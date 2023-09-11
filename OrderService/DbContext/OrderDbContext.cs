namespace OrderService.DbContext;
using Microsoft.EntityFrameworkCore;
using OrderService.Models;

public class OrderDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=OrderDb;Username=postgres;Password=nigaR123");
    }
        
    public DbSet<Order> Orders => Set<Order>();
}
