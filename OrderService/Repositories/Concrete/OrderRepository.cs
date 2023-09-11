using Microsoft.EntityFrameworkCore;
using OrderService.DbContext;
using OrderService.Models;
using OrderService.Repositories.Abstraction;
using System;
using System.Linq.Expressions;

namespace OrderService.Repositories.Concrete;

public class OrderRepository : IOrderRepository
{
    protected readonly OrderDbContext _dbContext;
    public OrderRepository(OrderDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public OrderRepository()
    {
        _dbContext = new OrderDbContext();
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        return await _dbContext.Orders.FindAsync(id);
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await _dbContext.Set<Order>().OrderBy(c => c.Id).ToListAsync();
    }

    public async Task<IEnumerable<Order>> FindAsync(Expression<Func<Order, bool>> expression)
    {
        return await _dbContext.Set<Order>().Where(expression).ToListAsync();
    }

    public async Task AddAsync(Order entity)
    {
        _dbContext.Set<Order>().Add(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Order entity)
    {
        _dbContext.Set<Order>().Update(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateRangeAsync(IEnumerable<Order> entities)
    {
        _dbContext.Set<Order>().UpdateRange(entities);
        await _dbContext.SaveChangesAsync();
    }

    public async Task AddRangeAsync(IEnumerable<Order> entities)
    {
        _dbContext.Set<Order>().AddRange(entities);
        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveAsync(Order entity)
    {
        _dbContext.Set<Order>().Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveRangeAsync(IEnumerable<Order> entities)
    {
        _dbContext.Set<Order>().RemoveRange(entities);
        await _dbContext.SaveChangesAsync();
    }
}
