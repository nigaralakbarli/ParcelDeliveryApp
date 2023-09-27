namespace DeliveryMicroservice.DbContext;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

public class DeliveryDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=database3;Port=5432;Database=delivery_db;User Id=postgres;Password=mypassword;");
    }

    public DbSet<Order> Orders => Set<Order>();
}
