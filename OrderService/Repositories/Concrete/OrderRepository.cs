using Microsoft.EntityFrameworkCore;
using OrderMicroservice.DbContext;
using OrderMicroservice.Repositories.Abstraction;
using Shared.Models;

namespace OrderMicroservice.Repositories.Concrete;

public class OrderRepository : BaseRepository<Order>, IOrderRepository
{
    public async Task<IEnumerable<Order>> GetAllIncludeAsync()
    {
        return await _dbContext.Orders
            .OrderBy(b => b.Id)
            .Include(b => b.StatusChanges)
            .ToListAsync();
    }

    public async Task<Order> GetByIdIncludeAsync(int id)
    {
        return await _dbContext.Orders
            .OrderBy(b => b.Id)
            .Include(b => b.StatusChanges).FirstOrDefaultAsync();
    }
}

