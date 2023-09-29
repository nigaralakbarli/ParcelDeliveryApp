using DeliveryMicroservice.Repositories.Abstraction;
using Shared.Models;

namespace DeliveryMicroservice.Repositories.Concrete;

public class OrderRepository : BaseRepository<Order>, IOrderRepository
{
}
