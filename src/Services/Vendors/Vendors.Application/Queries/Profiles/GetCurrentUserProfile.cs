using LoadLogic.Services.Vendors.Application.Models.Profiles;
using LoadLogic.Services.Vendors.Application.Interfaces;
using LoadLogic.Services.Vendors.Application.Models.MinorityTypes;
using LoadLogic.Services.Vendors.Application.Models;
using LoadLogic.Services.Vendors.Application.Models.ProductTypes;
using LoadLogic.Services.Vendors.Application.Models.Regions;
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

namespace LoadLogic.Services.Vendors.Application.Queries.Profiles
{
    /// <summary>
    /// An immutable query message for requesting a profile for the current user.
    /// </summary>
    public sealed class GetCurrentUserProfile : IRequest<ProfileDto>
    {
        public GetCurrentUserProfile() { }
    }

    internal class GetCurrentUserProfileHandler : IRequestHandler<GetCurrentUserProfile, ProfileDto>
    {
        private readonly IConnectionProvider _provider;

        public GetCurrentUserProfileHandler(IConnectionProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public async Task<ProfileDto> Handle(GetCurrentUserProfile request, CancellationToken cancellationToken)
        {
            var parentQuery = @"
                SELECT p.[Id]
                    ,p.[Name] 
                    ,p.[WebAddress] 
                    ,p.[CommunicationMethod]
                    ,p.PhoneNumber_Number [PhoneNumber]
                    ,p.FaxNumber_Number [FaxNumber]
                    ,p.ProfileAccentColor [ProfileAccentColor]
                    ,p.ProfileLogoUrl [ProfileLogoUrl]

                    ,p.[PrimaryAddress_AddressLine1] [AddressLine1]
                    ,p.[PrimaryAddress_AddressLine2] [AddressLine2]
                    ,p.[PrimaryAddress_Building] [Building]
                    ,p.[PrimaryAddress_City] [City]
                    ,p.[PrimaryAddress_StateProvince] [StateProvince]
                    ,p.[PrimaryAddress_CountryRegion] [CountryRegion]
                    ,p.[PrimaryAddress_PostalCode] [PostalCode]

                    ,p.[AlternateAddress_AddressLine1] [AddressLine1]
                    ,p.[AlternateAddress_AddressLine2] [AddressLine2]
                    ,p.[AlternateAddress_Building] [Building]
                    ,p.[AlternateAddress_City] [City]
                    ,p.[AlternateAddress_StateProvince] [StateProvince]
                    ,p.[AlternateAddress_CountryRegion] [CountryRegion]
                    ,p.[AlternateAddress_PostalCode] [PostalCode]
                    
                    ,r.[Id]
                    ,r.[Code]
                    ,r.[Description]
                FROM Profiles p
                LEFT JOIN Regions r ON r.[Id] = p.[RegionId]
                ";

            var childQuery = @"               
                SELECT pms.[Id]
                    ,pms.[ProfileId] [CompanyId]
                    ,pms.[CertificationNumber]
                    ,pms.[Percent]
                    
                    ,m.[Code]
                    ,m.[Id]
                    ,m.[Description]
                FROM ProfileMinorityStatuses pms
                LEFT JOIN MinorityTypes m ON m.[Id] = pms.[TypeId]
                WHERE pms.[ProfileId] = @Id;

                SELECT pp.[Id]
                    ,pp.[ProfileId] [CompanyId]
                    
                    ,pt.[Code]
                    ,pt.[Id]
                    ,pt.[Description]
                    
                    ,r.[Code]
                    ,r.[Id]
                    ,r.[Description]
                FROM ProfileProducts pp
                LEFT JOIN ProductTypes pt ON pt.[Id] = pp.[TypeId]
                LEFT JOIN Regions r ON r.[Id] = pp.[RegionId]
                WHERE pp.[ProfileId] = @Id;

                SELECT pc.[Id]
                    ,pc.[FirstName]
                    ,pc.[LastName]
                    ,pc.[Title]
                    ,pc.[IsMainContact]
                    ,pc.[Note]
                    ,pc.[PhoneNumber_Number] [PhoneNumber]
                    ,pc.[FaxNumber_Number] [FaxNumber]
                    ,pc.[CellPhoneNumber_Number] [CellPhoneNumber]

                    ,pc.[EmailAddress_Identifier] [Identifier]
                    ,pc.[EmailAddress_Domain] [Domain]
                FROM ProfileContacts pc
                WHERE pc.[ProfileId] = @Id;
                ";

            var parentParams = new { };

            using var connection = _provider.GetDbConnection();
            connection.Open();

            var result = await connection.QueryAsync<ProfileDto, Address, Address, RegionDto, ProfileDto>(
                parentQuery,
                (c, pa, aa, r) =>
                {
                    c.PrimaryAddress = pa;
                    c.AlternateAddress = aa;
                    c.Region = r;
                    return c;
                },
                parentParams,
                splitOn: "AddressLine1, AddressLine1, Id"
            );

            var profile = result.SingleOrDefault();
            if (profile == null)
            {
                throw new NotFoundException(nameof(Profile), "");
            }

            var childParams = new { profile.Id };
            using var reader = await connection.QueryMultipleAsync(childQuery, childParams);
            profile.MinorityStatuses = ReadStatuses(reader);
            profile.Products = ReadProducts(reader);
            profile.Contacts = ReadContacts(reader);
            return profile;
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
                splitOn: "Identifier"
            ).ToList();
        }
    }
}
