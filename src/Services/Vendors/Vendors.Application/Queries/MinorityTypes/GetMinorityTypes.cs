using MediatR;
using System.Collections.Generic;
using Dapper;
using System;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Vendors.Application.Models.MinorityTypes;
using LoadLogic.Services.Vendors.Application.Interfaces;

namespace LoadLogic.Services.Vendors.Application.Queries.MinorityTypes
{
    /// <summary>
    /// An immutable query message for getting all minority types.
    /// </summary>
    public sealed class GetMinorityTypes : IRequest<IEnumerable<MinorityTypeDto>>
    {
        public GetMinorityTypes()
        {
        }
    }


    internal class GetMinorityTypesHandler : IRequestHandler<GetMinorityTypes, IEnumerable<MinorityTypeDto>>
    {
        private readonly IConnectionProvider _provider;

        public GetMinorityTypesHandler(IConnectionProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public async Task<IEnumerable<MinorityTypeDto>> Handle(GetMinorityTypes request, CancellationToken cancellationToken)
        {
            var query = @"
                SELECT m.[Id], m.[Code], m.[Description]
                FROM MinorityTypes m
                ";

            var parameters = new { };

            using var connection = _provider.GetDbConnection();
            connection.Open();

            return await connection.QueryAsync<MinorityTypeDto>(query, parameters);
        }
    }
}
