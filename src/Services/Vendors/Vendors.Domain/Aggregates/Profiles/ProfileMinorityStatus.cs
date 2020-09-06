using System;

namespace LoadLogic.Services.Vendors.Domain
{
    public class ProfileMinorityStatus : MinorityStatus
    {
        public ProfileMinorityStatus(Profile profile, MinorityType type, string certificationNumber, decimal percent)
            : this(default, profile, type, certificationNumber, percent)
        {
        }

        public ProfileMinorityStatus(long id, Profile profile, MinorityType type, string certificationNumber, decimal percent)
        {
            this.Id = id;
            this.Profile = profile;
            this.ProfileId = profile.Id;
            this.TypeId = type.Id;
            this.Type = type;
            this.CertificationNumber = certificationNumber;
            this.Percent = percent;
        }

        public long ProfileId { get; private set; }
        public Profile Profile { get; private set; }

        public void Update(MinorityType type, string certificationNumber, decimal percent)
        {
            this.TypeId = type.Id;
            this.Type = type;
            this.CertificationNumber = certificationNumber;
            this.Percent = percent;
        }

#nullable disable
        private ProfileMinorityStatus() { }
#nullable restore
    }
}
