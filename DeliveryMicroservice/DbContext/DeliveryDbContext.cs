namespace DeliveryMicroservice.DbContext;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

public class DeliveryDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=deliverydb;Port=5432; Database=delivery_db;User Id=postgres;Password=mypassword;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Delivery>()
            .Property(a => a.DeliveryStatus)
            .HasConversion<string>();

        modelBuilder.Entity<DeliveryStatusChange>()
            .Property(a => a.NewStatus)
            .HasConversion<string>();

        base.OnModelCreating(modelBuilder);
    }
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<Delivery> Deliveries => Set<Delivery>();
}
