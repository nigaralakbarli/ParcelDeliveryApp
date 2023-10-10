using OrderMicroservice.Repositories.Abstraction;
using Shared.Models;

namespace OrderMicroservice.Repositories.Concrete;

public class OrderStatusChangeRepository : BaseRepository<OrderStatusChange>, IOrderStatusChangeRepository
{
}
