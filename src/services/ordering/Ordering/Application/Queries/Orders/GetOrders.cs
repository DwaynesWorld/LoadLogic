using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using LoadLogic.Services.Ordering.Application.Abstractions;
using LoadLogic.Services.Ordering.Application.Models.Orders;
using LoadLogic.Services.Ordering.Domain.Aggregates.Orders;
using MediatR;

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
                    ,o.CustomerPhone_Number [CustomerPhone]

                    ,o.[Type]

                    ,o.CustomerEmail_Identifier [Identifier]
                    ,o.CustomerEmail_Domain [Domain]
                    
                FROM Orders o
                ";

            using var connection = _provider.GetDbConnection();
            connection.Open();

            var orders = await connection.QueryAsync<OrderSummaryDto, int, Email, OrderSummaryDto>(
                query,
                (o, type, e) =>
                {
                    o.Type = Enumeration.FromValue<OrderType>(type);
                    o.CustomerEmail = e;
                    return o;
                },
                splitOn: "Type, Identifier"
            );

            return orders;
        }
    }
}
