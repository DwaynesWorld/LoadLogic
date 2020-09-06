namespace LoadLogic.Services.Vendors.Domain
{
    public partial class Profile
    {
        public static string IncludeRegion = $"{nameof(Profile.Region)}";
        public static string IncludeVendors = $"{nameof(Profile.Contacts)}";
        public static string IncludeStatuses = $"{nameof(Profile.MinorityStatuses)}.{nameof(ProfileMinorityStatus.Type)}";
        public static string[] IncludeProducts = { $"{nameof(Profile.Products)}.{nameof(ProfileProduct.Type)}", $"{nameof(Profile.Products)}.{nameof(ProfileProduct.Region)}" };
    }
}
