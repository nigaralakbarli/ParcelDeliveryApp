using DeliveryMicroservice.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace DeliveryMicroservice.Repositories.Concrete;

public class DeliveryRepository : BaseRepository<Delivery>, IDeliveryRepository
{
    public async Task<IEnumerable<Delivery>> GetAllIncludeAsync()
    {
        return await _dbContext.Deliveries
            .OrderBy(b => b.Id)
            .Include(b => b.Coordinates)
            .ToListAsync();
    }

    public async Task<Delivery> GetByIdIncludeAsync(int id)
    {
        return await _dbContext.Deliveries
            .OrderBy(b => b.Id)
            .Include(b => b.Coordinates)
            .FirstOrDefaultAsync();
    }
}
