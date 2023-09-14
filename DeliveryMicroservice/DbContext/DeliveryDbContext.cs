namespace DeliveryMicroservice.DbContext;
using Microsoft.EntityFrameworkCore;

public class DeliveryDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=OrderDb;Username=postgres;Password=nigaR123");
    }

    public DbSet<Delivery> Orders => Set<Order>();

}
