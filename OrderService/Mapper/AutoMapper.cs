using AutoMapper;
using OrderMicroservice.Dtos.Order;
using OrderMicroservice.Models;

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