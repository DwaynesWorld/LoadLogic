using MediatR;
using Dapper;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Vendors.Application.Models.ProductTypes;
using LoadLogic.Services.Vendors.Application.Interfaces;

namespace LoadLogic.Services.Vendors.Application.Queries.ProductTypes
{
    /// <summary>
    /// An immutable query message for requesting all Product Types.
    /// </summary>
    public sealed class GetProductTypes : IRequest<IEnumerable<ProductTypeDto>>
    {
        public GetProductTypes()
        {
        }
    }


    internal class GetProductTypesHandler : IRequestHandler<GetProductTypes, IEnumerable<ProductTypeDto>>
    {
        private readonly IConnectionProvider _provider;

        public GetProductTypesHandler(IConnectionProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public async Task<IEnumerable<ProductTypeDto>> Handle(GetProductTypes request, CancellationToken cancellationToken)
        {
            var query = @"
                SELECT p.[Id], p.[Code], p.[Description] 
                FROM ProductTypes p
                ";

            var parameters = new { };

            using var connection = _provider.GetDbConnection();
            connection.Open();

            return await connection.QueryAsync<ProductTypeDto>(query, parameters);
        }
    }
}
