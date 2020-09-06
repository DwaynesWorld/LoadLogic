using System.ComponentModel.DataAnnotations;
using LoadLogic.Services.Vendors.Application.Interfaces;
using LoadLogic.Services.Vendors.Application.Models;
using LoadLogic.Services.Vendors.Application.Models.CompanyTypes;
using LoadLogic.Services.Vendors.Application.Models.MinorityTypes;
using LoadLogic.Services.Vendors.Application.Models.ProductTypes;
using LoadLogic.Services.Vendors.Application.Models.Regions;
using LoadLogic.Services.Vendors.Application.Models.Vendors;
using LoadLogic.Services.Vendors.Domain;
using Dapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Dapper.SqlMapper;
using LoadLogic.Services.Exceptions;

namespace LoadLogic.Services.Vendors.Application.Queries.Vendors
{
    /// <summary>
    /// An immutable query message for requesting a vendor by its unique code.
    /// </summary>
    public sealed class GetVendorByCode : IRequest<VendorDto>
    {
        public GetVendorByCode(string code)
        {
            this.Code = code;
        }

        /// <summary>
        /// The vendor's unique code.
        /// </summary>
        [Required]
        public string Code { get; }

    }


    internal class GetVendorByCodeHandler : IRequestHandler<GetVendorByCode, VendorDto>
    {
        private readonly IConnectionProvider _provider;

        public GetVendorByCodeHandler(IConnectionProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public async Task<VendorDto> Handle(
            GetVendorByCode request,
            CancellationToken cancellationToken)
        {
            var parentQuery = @"
                SELECT v.[Id]
                    ,v.[Code]
                    ,v.[Name] 
                    ,v.[WebAddress] 
                    ,v.[IsBonded]
                    ,v.[BondRate]
                    ,v.PhoneNumber_Number [PhoneNumber]
                    ,v.FaxNumber_Number [FaxNumber]
                    ,v.[CommunicationMethod]
                    ,v.[Note]

                    ,v.PrimaryAddress_AddressLine1 [AddressLine1]
                    ,v.PrimaryAddress_AddressLine2 [AddressLine2]
                    ,v.PrimaryAddress_Building [Building]
                    ,v.PrimaryAddress_City [City]
                    ,v.PrimaryAddress_StateProvince [StateProvince]
                    ,v.PrimaryAddress_CountryRegion [CountryRegion]
                    ,v.PrimaryAddress_PostalCode [PostalCode]

                    ,v.AlternateAddress_AddressLine1 [AddressLine1]
                    ,v.AlternateAddress_AddressLine2 [AddressLine2]
                    ,v.AlternateAddress_Building [Building]
                    ,v.AlternateAddress_City [City]
                    ,v.AlternateAddress_StateProvince [StateProvince]
                    ,v.AlternateAddress_CountryRegion [CountryRegion]
                    ,v.AlternateAddress_PostalCode [PostalCode]

                    ,r.[Id]
                    ,r.[Code]
                    ,r.[Description]

                    ,t.[Id]
                    ,t.[Code]
                    ,t.[Description]
                FROM Vendors v
                LEFT JOIN Regions r ON r.[Id] = v.[RegionId]
                LEFT JOIN CompanyTypes t ON t.[Id] = v.[TypeId]
                WHERE v.[Code] = @Code;
                ";

            var childQuery = @"
                SELECT vms.[Id]
                    ,vms.[VendorId] [CompanyId]
                    ,vms.[CertificationNumber]
                    ,vms.[Percent]
                    
                    ,m.[Code]
                    ,m.[Id]
                    ,m.[Description]
                FROM VendorMinorityStatuses vms
                LEFT JOIN MinorityTypes m ON m.[Id] = vms.[TypeId]
                WHERE vms.[VendorId] = @Id;

                SELECT vp.[Id]
                    ,vp.[VendorId] [CompanyId]
                    
                    ,pt.[Code]
                    ,pt.[Id]
                    ,pt.[Description]
                    
                    ,r.[Code]
                    ,r.[Id]
                    ,r.[Description]
                FROM VendorProducts vp
                LEFT JOIN ProductTypes pt ON pt.[Id] = vp.[TypeId]
                LEFT JOIN Regions r ON r.[Id] = vp.[RegionId]
                WHERE vp.[VendorId] = @Id;

                SELECT vc.[Id]
                    ,vc.[FirstName]
                    ,vc.[LastName]
                    ,vc.[Title]
                    ,vc.[IsMainContact]
                    ,vc.[Note]
                    ,vc.[PhoneNumber_Number] [PhoneNumber]
                    ,vc.[FaxNumber_Number] [FaxNumber]
                    ,vc.[CellPhoneNumber_Number] [CellPhoneNumber]

                    ,vc.[EmailAddress_Identifier] [Identifier]
                    ,vc.[EmailAddress_Domain] [Domain]
                FROM VendorVendors vc
                WHERE vc.[VendorId] = @Id;
                ";

            var parentParameters = new { request.Code };

            using var connection = _provider.GetDbConnection();
            connection.Open();

            var result = await connection.QueryAsync<VendorDto, Address, Address, RegionDto, CompanyTypeDto, VendorDto>(
                parentQuery,
                (vendor, pa, aa, r, t) =>
                {
                    vendor.PrimaryAddress = pa;
                    vendor.AlternateAddress = aa;
                    vendor.Region = r;
                    vendor.Type = t;
                    return vendor;
                },
                parentParameters,
                splitOn: "AddressLine1, AddressLine1, Id, Id"
            );

            var vendor = result.SingleOrDefault();
            if (vendor == null)
            {
                throw new NotFoundException(nameof(Vendor), request.Code);
            }

            var childParameters = new { vendor.Id };

            using var childReader = await connection.QueryMultipleAsync(childQuery, childParameters);
            vendor.MinorityStatuses = ReadStatuses(childReader);
            vendor.Products = ReadProducts(childReader);
            vendor.Vendors = ReadVendors(childReader);
            return vendor;
        }

        private List<MinorityStatusDto> ReadStatuses(GridReader reader)
        {
            return reader.Read<MinorityStatusDto, MinorityTypeDto, MinorityStatusDto>(
                (c, cc) =>
                {
                    c.Type = cc;
                    return c;
                },
                splitOn: "Code"
            ).ToList();
        }

        private List<ProductDto> ReadProducts(GridReader reader)
        {
            return reader.Read<ProductDto, ProductTypeDto, RegionDto, ProductDto>(
                (cp, p, r) =>
                {
                    cp.Product = p;
                    cp.Region = r;
                    return cp;
                },
                splitOn: "Code, Code"
            ).ToList();
        }

        private List<ContactDto> ReadVendors(GridReader reader)
        {
            return reader.Read<ContactDto, Email, ContactDto>(
                (cp, e) =>
                {
                    cp.EmailAddress = e;
                    return cp;
                },
                splitOn: "PhoneNumber, FaxNumber, CellPhoneNumber, Identifier"
            ).ToList();
        }
    }
}
