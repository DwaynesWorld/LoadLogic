using LoadLogic.Services.Vendors.Application.Models.Regions;
using LoadLogic.Services.Vendors.Application.Interfaces;
using Dapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LoadLogic.Services.Vendors.Application.Queries.Regions
{
    /// <summary>
    /// An immutable query message for requesting all regions.
    /// </summary>
    public sealed class GetRegions : IRequest<IEnumerable<RegionDto>>
    {
        public GetRegions()
        {
        }
    }


    internal class GetRegionsHandler : IRequestHandler<GetRegions, IEnumerable<RegionDto>>
    {
        private readonly IConnectionProvider _provider;

        public GetRegionsHandler(IConnectionProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public async Task<IEnumerable<RegionDto>> Handle(GetRegions request, CancellationToken cancellationToken)
        {
            var query = @"
                SELECT r.[Id], r.[Code], r.[Description]
                FROM Regions r
                ";

            var parameters = new { };

            using var connection = _provider.GetDbConnection();
            connection.Open();

            return await connection.QueryAsync<RegionDto>(query, parameters);
        }
    }
}
