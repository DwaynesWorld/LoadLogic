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
    public sealed class GetOrders : IRequest<IEnumerable<OrderSummaryDto>>
    {
        public GetOrders() { }
    }


    internal class GetOrdersHandler : IRequestHandler<GetOrders, IEnumerable<OrderSummaryDto>>
    {
        private readonly IConnectionProvider _provider;

        public GetOrdersHandler(IConnectionProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public async Task<IEnumerable<OrderSummaryDto>> Handle(GetOrders request, CancellationToken cancellationToken)
        {
            var query = @"
                SELECT o.[Id]
                    ,o.[OrderNo]
                    ,o.[CustomerId]
                    ,o.[CustomerFirstName] 
                    ,o.[CustomerLastName] 
                    ,o.CustomerEmail_Identifier [Identifier]
                    ,o.CustomerEmail_Domain [Domain]
                    ,o.CustomerPhone_Number [CustomerPhone]
                    ,o.[JobName] 
                    ,o.[JobDescription] 
                    ,o.[JobStartDate] 
                    
                    ,o.[Type]

                    ,o.JobAddress_AddressLine1 [AddressLine1]
                    ,o.JobAddress_AddressLine2 [AddressLine2]
                    ,o.JobAddress_Building [Building]
                    ,o.JobAddress_City [City]
                    ,o.JobAddress_StateProvince [StateProvince]
                    ,o.JobAddress_CountryRegion [CountryRegion]
                    ,o.JobAddress_PostalCode [PostalCode]
                FROM Orders o
                ";

            using var connection = _provider.GetDbConnection();
            connection.Open();

            var vendors = await connection.QueryAsync<OrderSummaryDto, int, Address, OrderSummaryDto>(
                query,
                (o, type, a) =>
                {
                    o.Type = Enumeration.FromValue<OrderType>(type);
                    o.JobAddress = a;
                    return o;
                },
                splitOn: "Type, AddressLine1"
            );

            return vendors;
        }
    }
}
