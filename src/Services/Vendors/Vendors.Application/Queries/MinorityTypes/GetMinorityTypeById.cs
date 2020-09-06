using System.ComponentModel.DataAnnotations;
using LoadLogic.Services.Vendors.Domain;
using Dapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Vendors.Application.Models.MinorityTypes;
using LoadLogic.Services.Vendors.Application.Interfaces;
using LoadLogic.Services.Exceptions;

namespace LoadLogic.Services.Vendors.Application.Queries.MinorityTypes
{
    /// <summary>
    /// An immutable query message for getting a minority type by its unique identifier.
    /// </summary>
    public sealed class GetMinorityTypeById : IRequest<MinorityTypeDto>
    {
        public GetMinorityTypeById(long id)
        {
            this.Id = id;
        }

        /// <summary>
        /// The minority type's unique identifier.
        /// </summary>
        [Required]
        public long Id { get; }

    }

    internal class GetMinorityTypeByIdHandler : IRequestHandler<GetMinorityTypeById, MinorityTypeDto>
    {
        private readonly IConnectionProvider _provider;

        public GetMinorityTypeByIdHandler(IConnectionProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public async Task<MinorityTypeDto> Handle(GetMinorityTypeById request, CancellationToken cancellationToken)
        {
            var query = @"
                SELECT m.[Id], m.[Code], m.[Description]
                FROM MinorityTypes m
                WHERE m.[Id] = @Id
                ";

            var parameters = new { request.Id };

            using var connection = _provider.GetDbConnection();
            connection.Open();

            try
            {
                return await connection.QuerySingleAsync<MinorityTypeDto>(query, parameters);
            }
            catch (InvalidOperationException e)
            {
                if (e.Message == "Sequence contains no elements")
                {
                    throw new NotFoundException(nameof(MinorityType), request.Id, e);
                }

                throw;
            }
        }
    }
}
