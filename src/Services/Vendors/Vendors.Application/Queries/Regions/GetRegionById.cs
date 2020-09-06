using System.ComponentModel.DataAnnotations;
using LoadLogic.Services.Vendors.Application.Interfaces;
using LoadLogic.Services.Vendors.Application.Models.Regions;
using LoadLogic.Services.Vendors.Domain;
using Dapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Exceptions;

namespace LoadLogic.Services.Vendors.Application.Queries.Regions
{
    /// <summary>
    /// An immutable query messafe for requesting a region by its unique identifier.
    /// </summary>
    public sealed class GetRegionById : IRequest<RegionDto>
    {
        public GetRegionById(long id)
        {
            this.Id = id;
        }

        /// <summary>
        /// The unique identifier.
        /// </summary>
        [Required]
        public long Id { get; }
    }

    internal class GetRegionByIdHandler : IRequestHandler<GetRegionById, RegionDto>
    {
        private readonly IConnectionProvider _provider;

        public GetRegionByIdHandler(IConnectionProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public async Task<RegionDto> Handle(GetRegionById request, CancellationToken cancellationToken)
        {
            var query = @"
                SELECT r.[Id], r.[Code], r.[Description]
                FROM Regions r
                WHERE r.[Id] = @Id
                ";

            var parameters = new { request.Id };

            using var connection = _provider.GetDbConnection();
            connection.Open();

            try
            {
                return await connection.QuerySingleAsync<RegionDto>(query, parameters);
            }
            catch (InvalidOperationException e)
            {
                if (e.Message == "Sequence contains no elements")
                {
                    throw new NotFoundException(nameof(Region), request.Id, e);
                }

                throw;
            }
        }
    }

}
