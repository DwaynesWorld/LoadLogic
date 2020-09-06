using System.ComponentModel.DataAnnotations;
using LoadLogic.Services.Vendors.Application.Interfaces;
using LoadLogic.Services.Vendors.Application.Models;
using LoadLogic.Services.Vendors.Application.Models.CompanyTypes;
using LoadLogic.Services.Vendors.Application.Models.MinorityTypes;
using LoadLogic.Services.Vendors.Application.Models.ProductTypes;
using LoadLogic.Services.Vendors.Application.Models.Regions;
using LoadLogic.Services.Vendors.Application.Models.Vendors;
using Dapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace LoadLogic.Services.Vendors.Application.Queries.Vendors
{
    /// <summary>
    /// An immutable query message for requesting a detailed list of vendors .
    /// </summary>
    public sealed class GetDetailedVendors : IRequest<IEnumerable<VendorDto>>
    {
        public GetDetailedVendors()
        {
        }
    }


    internal class GetDetailedVendorsHandler : IRequestHandler<GetDetailedVendors, IEnumerable<VendorDto>>
    {
        private readonly IConnectionProvider _provider;

        public GetDetailedVendorsHandler(IConnectionProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public async Task<IEnumerable<VendorDto>> Handle(
            GetDetailedVendors request,
            CancellationToken cancellationToken)
        {
            var query = @"
                SELECT v.[Id]
                    ,v.[Code]
                    ,v.[Name] 
                    ,v.[WebAddress] 
                    ,v.[IsBonded]
                    ,v.[BondRate]
                    ,v.[CommunicationMethod]
                    ,v.[Note]
                    ,v.PhoneNumber_Number [PhoneNumber]
                    ,v.FaxNumber_Number [FaxNumber]

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
                LEFT JOIN CompanyTypes t ON t.[Id] = v.[TypeId];

                SELECT vms.[Id]
                    ,vms.[VendorId] [CompanyId]
                    ,vms.[CertificationNumber]
                    ,vms.[Percent]
                    
                    ,m.[Code]
                    ,m.[Id]
                    ,m.[Description]
                FROM VendorMinorityStatuses vms
                LEFT JOIN MinorityTypes m ON m.[Id] = vms.[TypeId];

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
                LEFT JOIN Regions r ON r.[Id] = vp.[RegionId];

                SELECT vc.[Id]
                    ,vc.[VendorId] [CompanyId]
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
                FROM VendorVendors vc;
                ";

            var parameters = new { };

            using var connection = _provider.GetDbConnection();
            connection.Open();

            using var reader = await connection.QueryMultipleAsync(query, parameters);

            var vendors = ReadVendor(reader);
            ReadStatuses(reader, vendors);
            ReadProducts(reader, vendors);
            ReadVendors(reader, vendors);
            return vendors.Values.ToList();
        }

        private Dictionary<long, VendorDto> ReadVendor(GridReader reader)
        {
            return reader.Read<VendorDto, Address, Address, RegionDto, CompanyTypeDto, VendorDto>(
                (c, pa, aa, r, t) =>
                {
                    c.PrimaryAddress = pa;
                    c.AlternateAddress = aa;
                    c.Region = r;
                    c.Type = t;
                    return c;
                },
                splitOn: "AddressLine1, AddressLine1, Id, Id"
            ).ToDictionary(k => k.Id, v => v);
        }

        private void ReadStatuses(GridReader reader, Dictionary<long, VendorDto> vendors)
        {
            var statusGroups = reader.Read<MinorityStatusDto, MinorityTypeDto, MinorityStatusDto>(
                (c, cc) =>
                {
                    c.Type = cc;
                    return c;
                },
                splitOn: "Code")
            .ToList()
            .GroupBy(s => s.CompanyId);

            foreach (var group in statusGroups)
            {
                vendors[group.Key].MinorityStatuses = group.ToList();
            }
        }

        private void ReadProducts(GridReader reader, Dictionary<long, VendorDto> vendors)
        {
            var productGroups = reader.Read<ProductDto, ProductTypeDto, RegionDto, ProductDto>(
                (cp, p, r) =>
                {
                    cp.Product = p;
                    cp.Region = r;
                    return cp;
                },
                splitOn: "Code, Code")
            .ToList()
            .GroupBy(p => p.CompanyId);

            foreach (var group in productGroups)
            {
                vendors[group.Key].Products = group.ToList();
            }
        }

        private void ReadVendors(GridReader reader, Dictionary<long, VendorDto> vendors)
        {
            var contactGroups = reader.Read<ContactDto, Email, ContactDto>(
                (cp, e) =>
                {
                    cp.EmailAddress = e;
                    return cp;
                },
                splitOn: "Identifier")
            .ToList()
            .GroupBy(c => c.CompanyId);

            foreach (var group in contactGroups)
            {
                vendors[group.Key].Vendors = group.ToList();
            }
        }
    }
}
