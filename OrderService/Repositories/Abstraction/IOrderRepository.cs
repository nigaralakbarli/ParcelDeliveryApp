using Shared.Models;

namespace OrderMicroservice.Repositories.Abstraction;

public interface IOrderRepository : IBaseRepository<Order>
{
    Task<Order> GetByIdIncludeAsync(int id);
    Task<IEnumerable<Order>> GetAllIncludeAsync();
}
