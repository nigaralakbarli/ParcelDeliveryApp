using AutoMapper;
using DeliveryMicroservice.Repositories.Abstraction;
using DeliveryMicroservice.Services.Abstraction;

namespace DeliveryMicroservice.Services.Concrete;

public class DeliveryService : IDeliveryService
{
    private readonly IDeliveryRepository _deliveryRepository;
    private readonly IMapper _mapper;   

    public DeliveryService(
        IDeliveryRepository deliveryRepository,
        IMapper mapper)
    {
        _deliveryRepository = deliveryRepository;
        _mapper = mapper;
    }
    public Task<bool> AssignOrderAsync(int orderId, string CourierId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SetDelivered(int orderId)
    {
        throw new NotImplementedException();
    }
}
