using AutoMapper;
using Shared.Dtos.Order;
using Shared.Models;

namespace OrderMicroservice.Mapper;

public class AutoMapper : Profile
{
    public AutoMapper()
    {
        CreateMap<Order, OrderResponseDto>();
        CreateMap<OrderCreateDTO, Order>();
        CreateMap<OrderUpdateDto, Order>();
    }
}