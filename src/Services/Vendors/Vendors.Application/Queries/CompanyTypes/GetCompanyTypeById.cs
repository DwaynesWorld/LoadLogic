using System.ComponentModel.DataAnnotations;
using System;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Vendors.Domain;
using Dapper;
using MediatR;
using LoadLogic.Services.Vendors.Application.Models.CompanyTypes;
using LoadLogic.Services.Vendors.Application.Interfaces;
using LoadLogic.Services.Exceptions;

namespace LoadLogic.Services.Vendors.Application.Queries.CompanyTypes
{

    /// <summary>
    /// An immutable query message for requesting a company type by its unique identifier.
    /// </summary>
    public sealed class GetCompanyTypeById : IRequest<CompanyTypeDto>
    {
        public GetCompanyTypeById(long id)
        {
            this.Id = id;
        }

        /// <summary>
        /// The unique identifier used to find the company type.
        /// </summary>
        [Required]
        public long Id { get; }
    }

    internal class GetCompanyTypeByIdHandler : IRequestHandler<GetCompanyTypeById, CompanyTypeDto>
    {
        private readonly IConnectionProvider _provider;

        public GetCompanyTypeByIdHandler(IConnectionProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public async Task<CompanyTypeDto> Handle(GetCompanyTypeById request, CancellationToken cancellationToken)
        {
            var query = @"
                SELECT c.[Id], c.[Code], c.[Description]
                FROM CompanyTypes c
                WHERE c.[Id] = @Id
                ";

            var parameters = new { request.Id };

            using var connection = _provider.GetDbConnection();
            connection.Open();

            try
            {
                return await connection.QuerySingleAsync<CompanyTypeDto>(query, parameters);
            }
            catch (InvalidOperationException e)
            {
                if (e.Message == "Sequence contains no elements")
                {
                    throw new NotFoundException(nameof(CompanyType), request.Id, e);
                }

                throw;
            }
        }
    }
}
