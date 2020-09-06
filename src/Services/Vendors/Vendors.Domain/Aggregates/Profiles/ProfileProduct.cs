using System;

namespace LoadLogic.Services.Vendors.Domain
{
    public class ProfileProduct : Product
    {
        public ProfileProduct(Profile profile, ProductType type, Region? region)
            : this(default, profile, type, region)
        {
        }

        public ProfileProduct(long id, Profile profile, ProductType type, Region? region)
        {
            this.Id = id;
            this.Profile = profile;
            this.ProfileId = profile.Id;
            this.TypeId = type.Id;
            this.Type = type;
            this.RegionId = region?.Id;
            this.Region = region;
        }

        public long ProfileId { get; private set; }
        public Profile Profile { get; private set; }

        public void Update(ProductType type, Region? region)
        {
            this.TypeId = type.Id;
            this.Type = type;
            this.RegionId = region?.Id;
            this.Region = region;
        }

#nullable disable
        private ProfileProduct() { }
#nullable restore
    }
}
