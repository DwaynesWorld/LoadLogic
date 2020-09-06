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
    /// An immutable query message for requesting a vendor by its unique identifier.
    /// </summary>
    public sealed class GetVendorById : IRequest<VendorDto>
    {
        public GetVendorById(long id)
        {
            this.Id = id;
        }

        /// <summary>
        /// The vendor's unique identifier.
        /// </summary>
        [Required]
        public long Id { get; }

    }


    internal class GetVendorByIdHandler : IRequestHandler<GetVendorById, VendorDto>
    {
        private readonly IConnectionProvider _provider;

        public GetVendorByIdHandler(IConnectionProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public async Task<VendorDto> Handle(
            GetVendorById request,
            CancellationToken cancellationToken)
        {
            var query = @"
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
                WHERE v.[Id] = @Id;

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
                FROM VendorContacts vc
                WHERE vc.[VendorId] = @Id;
                ";

            var parameters = new { request.Id };

            using var connection = _provider.GetDbConnection();
            connection.Open();

            using var reader = await connection.QueryMultipleAsync(query, parameters);

            try
            {
                var vendor = ReadVendor(reader);
                vendor.MinorityStatuses = ReadStatuses(reader);
                vendor.Products = ReadProducts(reader);
                vendor.Contacts = ReadContacts(reader);
                return vendor;
            }
            catch (InvalidOperationException e)
            {
                if (e.Message == "Sequence contains no elements")
                {
                    throw new NotFoundException(nameof(Vendor), request.Id, e);
                }

                throw;
            }
        }

        private VendorDto ReadVendor(GridReader reader)
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
            ).Single();
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

        private List<ContactDto> ReadContacts(GridReader reader)
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
