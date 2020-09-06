using System;

namespace LoadLogic.Services.Vendors.Domain
{
    public class VendorContact : Contact
    {
        public VendorContact(
            Vendor vendor, string firstName, string lastName, string title,
            PhoneNumber? phoneNumber, PhoneNumber? faxNumber, PhoneNumber? cellPhoneNumber,
            Email? emailAddress, string note, bool isMainContact)
            : this(default, vendor, firstName, lastName, title,
                    phoneNumber, faxNumber, cellPhoneNumber,
                    emailAddress, note, isMainContact)
        {
        }

        public VendorContact(
            long id, Vendor vendor, string firstName, string lastName, string title,
            PhoneNumber? phoneNumber, PhoneNumber? faxNumber, PhoneNumber? cellPhoneNumber,
            Email? emailAddress, string note, bool isMainContact)
        {
            this.Id = id;
            this.Vendor = vendor;
            this.VendorId = vendor.Id;
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

        public long VendorId { get; private set; }
        public Vendor Vendor { get; private set; }

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
        /// This method should only be called by the vendor entity.
        /// </summary>
        internal void SetIsMainContact(bool isMainContact)
        {
            this.IsMainContact = isMainContact;
        }

#nullable disable
        private VendorContact() { }
#nullable restore
    }
}
