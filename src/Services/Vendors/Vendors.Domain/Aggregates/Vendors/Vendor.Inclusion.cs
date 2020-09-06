namespace LoadLogic.Services.Vendors.Domain
{
    public partial class Vendor
    {
        public static string IncludeCompanyType = $"{nameof(Vendor.Type)}";
        public static string IncludeRegion = $"{nameof(Vendor.Region)}";
        public static string IncludeVendors = $"{nameof(Vendor.Contacts)}";
        public static string IncludeStatuses = $"{nameof(Vendor.MinorityStatuses)}.{nameof(VendorMinorityStatus.Type)}";
        public static string[] IncludeProducts = { $"{nameof(Vendor.Products)}.{nameof(VendorProduct.Type)}", $"{nameof(Vendor.Products)}.{nameof(VendorProduct.Region)}" };
    }
}
