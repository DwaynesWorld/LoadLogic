namespace LoadLogic.Services.Vendors.Application.Models.Profiles
{
    /// <summary>
    /// A data transfer object for requesting the creation of a company contact.
    /// </summary>
    public class CreateProfileContactDto
    {
        /// <summary>
        /// The contact's first name.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// The contact's last name.
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// The contact's title.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// The contact's phone number.
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// The contact's fax number.
        /// </summary>
        public string FaxNumber { get; set; } = string.Empty;

        /// <summary>
        /// The contact's cell phone number.
        /// </summary>
        public string CellPhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// The contact's email address.
        /// </summary>
        public string EmailAddress { get; set; } = string.Empty;

        /// <summary>
        /// The contact's note.
        /// </summary>
        public string Note { get; set; } = string.Empty;

        /// <summary>
        /// A boolean indicating whether this contact is the company's main contact.
        /// </summary>
        public bool IsMainContact { get; set; }
    }
}
