using LoadLogic.Services.Vendors.Application.Interfaces;
using LoadLogic.Services.Vendors.Application.Models.Vendors;
using LoadLogic.Services.Vendors.Application.Models.Regions;
using LoadLogic.Services.Vendors.Domain;
using Dapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Vendors.Application.Models.CompanyTypes;

namespace LoadLogic.Services.Vendors.Application.Queries.Vendors
{
    /// <summary>
    /// An immutable query message for requesting all companies.
    /// </summary>
    public sealed class GetVendors : IRequest<IEnumerable<VendorSummaryDto>>
    {
        public GetVendors()
        {
        }
    }


    internal class GetVendorsHandler : IRequestHandler<GetVendors, IEnumerable<VendorSummaryDto>>
    {
        private readonly IConnectionProvider _provider;

        public GetVendorsHandler(IConnectionProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public async Task<IEnumerable<VendorSummaryDto>> Handle(GetVendors request, CancellationToken cancellationToken)
        {
            var query = @"
                SELECT v.[Id]
                    ,v.[Code]
                    ,v.[Name] 
                    ,v.PhoneNumber_Number [PhoneNumber]
                    ,v.[WebAddress] 
                    ,v.[IsBonded]
                    ,v.[BondRate]
                    ,v.[CommunicationMethod]
                    ,v.[Note]

                    ,v.PrimaryAddress_AddressLine1 [AddressLine1]
                    ,v.PrimaryAddress_AddressLine2 [AddressLine2]
                    ,v.PrimaryAddress_Building [Building]
                    ,v.PrimaryAddress_City [City]
                    ,v.PrimaryAddress_StateProvince [StateProvince]
                    ,v.PrimaryAddress_CountryRegion [CountryRegion]
                    ,v.PrimaryAddress_PostalCode [PostalCode]

                    ,r.[Id]
                    ,r.[Code]
                    ,r.[Description]

                    ,t.[Id]
                    ,t.[Code]
                    ,t.[Description]
                FROM Vendors v
                LEFT JOIN Regions r ON r.Id = v.RegionId
                LEFT JOIN CompanyTypes t ON t.Id = v.TypeId
                ";

            var parameters = new { };

            using var connection = _provider.GetDbConnection();
            connection.Open();

            var vendors = await connection.QueryAsync<VendorSummaryDto, Address, RegionDto, CompanyTypeDto, VendorSummaryDto>(
                query,
                (c, pa, r, t) =>
                {
                    c.PrimaryAddress = pa;
                    c.Region = r;
                    c.Type = t;
                    return c;
                },
                param: parameters,
                splitOn: "AddressLine1, Id, Id"
            );

            return vendors;
        }
    }
}
