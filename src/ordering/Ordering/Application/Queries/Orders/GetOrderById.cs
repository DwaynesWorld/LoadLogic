using LoadLogic.Services.Ordering.Application.Interfaces;
using LoadLogic.Services.Ordering.Application.Models.Orders;
using Dapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Ordering.Domain.Aggregates.Orders;

namespace LoadLogic.Services.Ordering.Application.Queries.Orders
{
    public sealed class GetOrderById : IRequest<IEnumerable<OrderDto>>
    {
        public GetOrderById(long id)
        {
            this.Id = id;
        }

        public long Id { get; }
    }


    internal class GetOrderByIdHandler : IRequestHandler<GetOrderById, IEnumerable<OrderDto>>
    {
        private readonly IConnectionProvider _provider;

        public GetOrderByIdHandler(IConnectionProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public async Task<IEnumerable<OrderDto>> Handle(GetOrderById request, CancellationToken cancellationToken)
        {
            // var query = @"
            //     SELECT o.[Id]
            //         ,o.[OrderNo]
            //         ,o.[CustomerId]
            //         ,o.[CustomerFirstName] 
            //         ,o.[CustomerLastName] 
            //         ,o.CustomerEmail_Identifier [Identifier]
            //         ,o.CustomerEmail_Domain [Domain]
            //         ,o.CustomerPhone_Number [CustomerPhone]
            //         ,o.[JobName] 
            //         ,o.[JobDescription] 
            //         ,o.[JobStartDate] 

            //         ,o.[Type]

            //         ,o.JobAddress_AddressLine1 [AddressLine1]
            //         ,o.JobAddress_AddressLine2 [AddressLine2]
            //         ,o.JobAddress_Building [Building]
            //         ,o.JobAddress_City [City]
            //         ,o.JobAddress_StateProvince [StateProvince]
            //         ,o.JobAddress_CountryRegion [CountryRegion]
            //         ,o.JobAddress_PostalCode [PostalCode]
            //     FROM Orders o
            //     WHERE o.Id = @Id;

            //     SELECT oli.[Id]
            //         ,oli.[OrderId]
            //         ,oli.[MaterialId]
            //         ,oli.[MaterialName]
            //         ,oli.[MaterialUnit]
            //         ,oli.[MaterialQuantity]
            //         ,oli.[TruckType]
            //         ,oli.[TruckQuantity]
            //         ,oli.[ChargeType]
            //         ,oli.[ChargeRate]

            //         ,r.
            //     FROM OrderLineItems oli
            //     JOIN Route r on r.OrderItemId = oli.Id
            //     JOIN RouteLegs rl on rl.RouteId = r.Id
            //     WHERE oli.OrderId = @Id;
            //     ";

            // var parameters = new
            // {
            //     request.Id,
            // };

            // using var connection = _provider.GetDbConnection();
            // connection.Open();

            // var orders = await connection.QueryAsync<OrderDto, int, Address, OrderDto>(
            //     query,
            //     (o, type, a) =>
            //     {
            //         o.Type = Enumeration.FromValue<OrderType>(type);
            //         o.JobAddress = a;
            //         return o;
            //     },
            //     param: parameters,
            //     splitOn: "Type, AddressLine1"
            // );

            return new List<OrderDto>();
        }
    }
}
