namespace LoadLogic.Services.Vendors.Domain
{
    public abstract class Contact : Entity
    {
        public string FirstName { get; protected set; } = string.Empty;
        public string LastName { get; protected set; } = string.Empty;
        public string Title { get; protected set; } = string.Empty;
        public PhoneNumber? PhoneNumber { get; protected set; }
        public PhoneNumber? FaxNumber { get; protected set; }
        public PhoneNumber? CellPhoneNumber { get; protected set; }
        public Email? EmailAddress { get; protected set; }
        public string Note { get; protected set; } = string.Empty;
        public bool IsMainContact { get; protected set; }
    }
}