using OrderService.Models;
using System.Linq.Expressions;

namespace OrderService.Repositories.Abstraction;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(int id);
    Task<IEnumerable<Order>> GetAllAsync();
    Task<IEnumerable<Order>> FindAsync(Expression<Func<Order, bool>> expression);
    Task AddAsync(Order entity);
    Task AddRangeAsync(IEnumerable<Order> entities);
    Task RemoveAsync(Order entity);
    Task RemoveRangeAsync(IEnumerable<Order> entities);
    Task UpdateAsync(Order entity);
}
