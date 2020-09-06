using System;

namespace LoadLogic.Services.Vendors.Domain
{
    public class VendorProduct : Product
    {
        public VendorProduct(Vendor vendor, ProductType type, Region? region)
            : this(default, vendor, type, region)
        {
        }

        public VendorProduct(long id, Vendor vendor, ProductType type, Region? region)
        {
            this.Id = id;
            this.Vendor = vendor;
            this.VendorId = vendor.Id;
            this.TypeId = type.Id;
            this.Type = type;
            this.RegionId = region?.Id;
            this.Region = region;
        }


        public long VendorId { get; private set; }
        public Vendor Vendor { get; private set; }

        public void Update(ProductType type, Region? region)
        {
            this.TypeId = type.Id;
            this.Type = type;
            this.RegionId = region?.Id;
            this.Region = region;
        }

#nullable disable
        private VendorProduct() { }
#nullable restore
    }
}
