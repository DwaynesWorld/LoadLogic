using System.ComponentModel.DataAnnotations;
using LoadLogic.Services.Vendors.Domain;
using Dapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Vendors.Application.Interfaces;
using LoadLogic.Services.Vendors.Application.Models.ProductTypes;
using LoadLogic.Services.Exceptions;

namespace LoadLogic.Services.Vendors.Application.Queries.ProductTypes
{
    /// <summary>
    /// An immutable query message for requesting a product type by unique identifier.
    /// </summary>
    public sealed class GetProductTypeById : IRequest<ProductTypeDto>
    {
        public GetProductTypeById(long id)
        {
            this.Id = id;
        }

        /// <summary>
        /// The unique identifier of the product type.
        /// </summary>
        [Required]
        public long Id { get; }
    }


    internal class GetProductTypeByIdHandler : IRequestHandler<GetProductTypeById, ProductTypeDto>
    {
        private readonly IConnectionProvider _provider;

        public GetProductTypeByIdHandler(IConnectionProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public async Task<ProductTypeDto> Handle(GetProductTypeById request, CancellationToken cancellationToken)
        {
            var query = @"
                SELECT p.[Id], p.[Code], p.[Description]
                FROM ProductTypes p
                WHERE p.[Id] = @Id
                ";

            var parameters = new { request.Id };

            using var connection = _provider.GetDbConnection();
            connection.Open();

            try
            {
                return await connection.QuerySingleAsync<ProductTypeDto>(query, parameters);
            }
            catch (InvalidOperationException e)
            {
                if (e.Message == "Sequence contains no elements")
                {
                    throw new NotFoundException(nameof(ProductType), request.Id, e);
                }

                throw;
            }
        }
    }
}
