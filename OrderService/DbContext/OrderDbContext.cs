﻿namespace OrderMicroservice.DbContext;

using Microsoft.EntityFrameworkCore;
using Shared.Models;

public class OrderDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=orderdb;Port=5432;Database=order_db;User Id=postgres;Password=mypassword;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>()
            .Property(a => a.OrderStatus)
            .HasConversion<string>();

        modelBuilder.Entity<OrderStatusChange>()
            .Property(a => a.NewStatus)
            .HasConversion<string>();

        base.OnModelCreating(modelBuilder);
    }
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderStatusChange> OrderStatusChanges => Set<OrderStatusChange>();
}

