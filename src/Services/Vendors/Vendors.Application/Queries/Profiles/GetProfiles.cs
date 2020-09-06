using LoadLogic.Services.Vendors.Application.Models.Profiles;
using LoadLogic.Services.Vendors.Application.Interfaces;
using LoadLogic.Services.Vendors.Application.Models.Regions;
using Dapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LoadLogic.Services.Vendors.Application.Queries.Profiles
{
    /// <summary>
    /// An immutable query message for requesting all profiles.
    /// </summary>
    public sealed class GetProfiles : IRequest<IEnumerable<ProfileSummaryDto>>
    {
        public GetProfiles() { }
    }


    internal class GetProfilesHandler : IRequestHandler<GetProfiles, IEnumerable<ProfileSummaryDto>>
    {
        private readonly IConnectionProvider _provider;

        public GetProfilesHandler(IConnectionProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public async Task<IEnumerable<ProfileSummaryDto>> Handle(
            GetProfiles request,
            CancellationToken cancellationToken)
        {
            var query = @"
                SELECT p.[Id]
                    ,p.[Name] 
                    ,p.[WebAddress] 
                    ,p.[CommunicationMethod]
                    ,p.PhoneNumber_Number [PhoneNumber]
                    ,p.ProfileAccentColor [ProfileAccentColor]
                    ,p.ProfileLogoUrl [ProfileLogoUrl]

                    ,p.PrimaryAddress_AddressLine1 [AddressLine1]
                    ,p.PrimaryAddress_AddressLine2 [AddressLine2]
                    ,p.PrimaryAddress_Building [Building]
                    ,p.PrimaryAddress_City [City]
                    ,p.PrimaryAddress_StateProvince [StateProvince]
                    ,p.PrimaryAddress_CountryRegion [CountryRegion]
                    ,p.PrimaryAddress_PostalCode [PostalCode]
                    
                    ,r.[Id]
                    ,r.[Code]
                    ,r.[Description]
                FROM Profiles p
                LEFT JOIN Regions r ON r.[Id] = p.[RegionId]";

            using var connection = _provider.GetDbConnection();
            connection.Open();

            var profiles = await connection.QueryAsync<ProfileSummaryDto, Address, RegionDto, ProfileSummaryDto>(
                query,
                (c, pa, r) =>
                {
                    c.PrimaryAddress = pa;
                    c.Region = r;
                    return c;
                },
                splitOn: "AddressLine1, Id"
            );

            return profiles;
        }
    }
}
