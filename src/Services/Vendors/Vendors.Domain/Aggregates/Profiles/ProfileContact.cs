namespace LoadLogic.Services.Vendors.Domain
{
    public class ProfileContact : Contact
    {
        public ProfileContact(
            Profile profile, string firstName, string lastName, string title,
            PhoneNumber? phoneNumber, PhoneNumber? faxNumber, PhoneNumber? cellPhoneNumber,
            Email? emailAddress, string note, bool isMainContact)
            : this(
                default, profile, firstName, lastName, title,
                phoneNumber, faxNumber, cellPhoneNumber,
                emailAddress, note, isMainContact)
        {
        }

        public ProfileContact(
            long id, Profile profile, string firstName, string lastName, string title,
            PhoneNumber? phoneNumber, PhoneNumber? faxNumber, PhoneNumber? cellPhoneNumber,
            Email? emailAddress, string note, bool isMainContact)
        {
            this.Id = id;
            this.Profile = profile;
            this.ProfileId = profile.Id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Title = title;
            this.PhoneNumber = phoneNumber;
            this.FaxNumber = faxNumber;
            this.CellPhoneNumber = cellPhoneNumber;
            this.EmailAddress = emailAddress;
            this.Note = note;
            this.IsMainContact = isMainContact;
        }


        public long ProfileId { get; private set; }
        public Profile Profile { get; private set; }

        public void Update(
            string firstName, string lastName, string title,
            PhoneNumber? phoneNumber, PhoneNumber? faxNumber, PhoneNumber? cellPhoneNumber,
            Email? emailAddress, string note, bool isMainContact)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Title = title;
            this.PhoneNumber = phoneNumber;
            this.FaxNumber = faxNumber;
            this.CellPhoneNumber = cellPhoneNumber;
            this.EmailAddress = emailAddress;
            this.Note = note;
            this.IsMainContact = isMainContact;
        }

        /// <summary>
        /// This method should only be called by the profile entity.
        /// </summary>
        internal void SetIsMainContact(bool isMainContact)
        {
            this.IsMainContact = isMainContact;
        }

#nullable disable
        private ProfileContact() { }
#nullable restore
    }
}
