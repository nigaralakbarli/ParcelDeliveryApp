using AutoMapper;
using Shared.Dtos.Order;
using Shared.Models;

namespace OrderMicroservice.Mapper;

public class AutoMapper : Profile
{
    public AutoMapper()
    {
        CreateMap<Order, OrderResponseDto>();
        CreateMap<OrderStatusChange, OrderStatusChangeDto>()
            .ForMember(dest => dest.NewStatus, opt =>
                opt.MapFrom(src => src.NewStatus.ToString()));
        CreateMap<OrderCreateDTO, Order>();
        CreateMap<OrderUpdateDto, Order>();
    }
}