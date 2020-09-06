namespace LoadLogic.Services.Vendors.Domain
{
    public class VendorMinorityStatus : MinorityStatus
    {
        public VendorMinorityStatus(Vendor vendor, MinorityType type, string certificationNumber, decimal percent)
            : this(default, vendor, type, certificationNumber, percent)
        {
        }

        public VendorMinorityStatus(long id, Vendor vendor, MinorityType type, string certificationNumber, decimal percent)
        {
            this.Id = id;
            this.Vendor = vendor;
            this.VendorId = vendor.Id;
            this.TypeId = type.Id;
            this.Type = type;
            this.CertificationNumber = certificationNumber;
            this.Percent = percent;
        }

        public long VendorId { get; private set; }
        public Vendor Vendor { get; private set; }

        public void Update(MinorityType type, string certificationNumber, decimal percent)
        {
            this.TypeId = type.Id;
            this.Type = type;
            this.CertificationNumber = certificationNumber;
            this.Percent = percent;
        }

#nullable disable
        private VendorMinorityStatus() { }
#nullable restore
    }
}
