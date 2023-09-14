using DeliveryMicroservice.Models;
using DeliveryMicroservice.Repositories.Abstraction;

namespace DeliveryMicroservice.Repositories.Concrete;

public class DeliveryRepository : BaseRepository<Delivery>, IDeliveryRepository
{
}
