using System.Collections.Generic;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using LoadLogic.Services.Vendors.Application.Models.CompanyTypes;
using LoadLogic.Services.Vendors.Application.Interfaces;

namespace LoadLogic.Services.Vendors.Application.Queries.CompanyTypes
{
    /// <summary>
    /// An immutable query message for getting all company types.
    /// </summary>
    public sealed class GetCompanyTypes : IRequest<IEnumerable<CompanyTypeDto>>
    {
        public GetCompanyTypes()
        {
        }
    }


    internal class GetCompanyTypesHandler : IRequestHandler<GetCompanyTypes, IEnumerable<CompanyTypeDto>>
    {
        private readonly IConnectionProvider _provider;

        public GetCompanyTypesHandler(IConnectionProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public async Task<IEnumerable<CompanyTypeDto>> Handle(GetCompanyTypes request, CancellationToken cancellationToken)
        {
            var query = @"
                SELECT c.[Id], c.[Code], c.[Description]
                FROM CompanyTypes c
                ";

            var parameters = new { };

            using var connection = _provider.GetDbConnection();
            connection.Open();

            return await connection.QueryAsync<CompanyTypeDto>(query, parameters);
        }
    }
}
