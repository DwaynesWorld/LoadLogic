namespace LoadLogic.Services.Vendors.API
{
    public class AuthSettings
    {
        public string Authority { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string ApiClientId { get; set; } = string.Empty;
        public string ApiClientSecret { get; set; } = string.Empty;
        public string ApiClientScopes { get; set; } = string.Empty;
    }

    public class ConnectionStrings
    {
        public string VendorsContext { get; set; } = string.Empty;
        public string StorageAccount { get; set; } = string.Empty;
    }
}
