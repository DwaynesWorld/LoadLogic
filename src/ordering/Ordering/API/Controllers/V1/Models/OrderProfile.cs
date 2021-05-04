using AutoMapper;
using LoadLogic.Services.Ordering.Application.Commands.Orders;
using LoadLogic.Services.Ordering.Application.Models.Orders;
using LoadLogic.Services.Ordering.Domain.Aggregates.Orders;

namespace LoadLogic.Services.Ordering.API.Controllers.V1.Models
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<CreateRouteLegApiRequest, CreateRouteLegDto>();
            CreateMap<CreateRouteApiRequest, CreateRouteDto>();
            CreateMap<CreateOrderLineItemApiRequest, CreateOrderLineItemDto>();

            CreateMap<int, OrderType>()
                .ConvertUsing(t => Enumeration.FromValue<OrderType>(t));
            CreateMap<OrderType, int>()
                .ConvertUsing(t => t.Id);
            CreateMap<CreateOrderApiRequest, CreateOrderCommand>();
            // .ForMember(d => d.Type, m => m.MapFrom(s => Enumeration.FromValue<OrderType>(s.Type)));
            CreateMap<OrderSummaryDto, OrderSummaryApiResponse>()
                .ForMember(d => d.Type, m => m.MapFrom(s => s.Type.Id));
        }
    }
}
