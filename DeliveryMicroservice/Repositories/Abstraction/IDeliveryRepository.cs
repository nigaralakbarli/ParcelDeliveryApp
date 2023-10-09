using Shared.Models;

namespace DeliveryMicroservice.Repositories.Abstraction;

public interface IDeliveryRepository : IBaseRepository<Delivery>
{
    Task<Delivery> GetByIdIncludeAsync(int id);
    Task<IEnumerable<Delivery>> GetAllIncludeAsync();

}
