using AutoMapper;
using Shared.Dtos.Delivery;
using Shared.Dtos.Order;
using Shared.Models;

namespace DeliveryMicroservice.Mapper;

public class AutoMapper : Profile
{
    public AutoMapper()
    {
        CreateMap<Delivery, DeliveryResponseDto>();
        CreateMap<DeliveryStatusChange, DeliveryStatusChangeDto>()
            .ForMember(dest => dest.NewStatus, opt =>
                opt.MapFrom(src => src.NewStatus.ToString()));
    }
}