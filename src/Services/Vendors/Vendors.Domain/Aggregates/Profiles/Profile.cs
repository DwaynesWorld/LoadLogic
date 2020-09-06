using System;
using System.Collections.Generic;
using System.Linq;
using LoadLogic.Services.Abstractions;
using LoadLogic.Services.Exceptions;
using LoadLogic.Services.Extensions;

namespace LoadLogic.Services.Vendors.Domain
{
    public partial class Profile : Company, IAggregateRoot
    {
        private readonly HashSet<ProfileMinorityStatus> _minorityStatuses;
        private readonly HashSet<ProfileContact> _contacts;
        private readonly HashSet<ProfileProduct> _products;

        public Profile(
            string name,
            Address? primaryAddress, Address? alternateAddress, PhoneNumber? phoneNumber, PhoneNumber? faxNumber,
            string webAddress, Region? region, CommunicationMethod communicationMethod, string profileAccentColor)
            : this(
                  default, name,
                  primaryAddress, alternateAddress, phoneNumber, faxNumber,
                  webAddress, region, communicationMethod, profileAccentColor)
        {
        }

        public Profile(
            long id, string name,
            Address? primaryAddress, Address? alternateAddress,
            PhoneNumber? phoneNumber, PhoneNumber? faxNumber,
            string webAddress, Region? region, CommunicationMethod communicationMethod, string profileAccentColor)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Company name is required.", nameof(name));
            }

            if (!string.IsNullOrWhiteSpace(profileAccentColor) && !profileAccentColor.IsValidHexColor())
            {
                throw new InvalidHexColorException(profileAccentColor);
            }

            this.Id = id;
            this.Name = name;
            this.PrimaryAddress = primaryAddress;
            this.AlternateAddress = alternateAddress;
            this.PhoneNumber = phoneNumber;
            this.FaxNumber = faxNumber;
            this.WebAddress = webAddress;
            this.Region = region;
            this.CommunicationMethod = communicationMethod;
            this.ProfileAccentColor = profileAccentColor;

            _minorityStatuses = new HashSet<ProfileMinorityStatus>();
            _contacts = new HashSet<ProfileContact>();
            _products = new HashSet<ProfileProduct>();
        }

        public IReadOnlyCollection<ProfileMinorityStatus> MinorityStatuses => _minorityStatuses;
        public IReadOnlyCollection<ProfileContact> Contacts => _contacts;
        public IReadOnlyCollection<ProfileProduct> Products => _products;

        public string ProfileLogoUrl { get; private set; } = string.Empty;
        public string ProfileAccentColor { get; private set; }

        public void Update(
            string name, Address? primaryAddress, Address? alternateAddress,
            PhoneNumber? phoneNumber, PhoneNumber? faxNumber,
            string webAddress, Region? region, CommunicationMethod communicationMethod, string profileAccentColor)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Company name is required.", nameof(name));
            }

            if (!string.IsNullOrWhiteSpace(profileAccentColor) && !profileAccentColor.IsValidHexColor())
            {
                throw new InvalidHexColorException(profileAccentColor);
            }

            this.Name = name;
            this.PrimaryAddress = primaryAddress;
            this.AlternateAddress = alternateAddress;
            this.PhoneNumber = phoneNumber;
            this.FaxNumber = faxNumber;
            this.WebAddress = webAddress;
            this.Region = region;
            this.CommunicationMethod = communicationMethod;
            this.ProfileAccentColor = profileAccentColor;
        }

        public void UpdateLogoUrl(string url)
        {
            this.ProfileLogoUrl = url;
        }

        public void AddMinorityStatus(ProfileMinorityStatus status)
        {
            _minorityStatuses.Add(status);
        }

        public void RemoveStatus(long id)
        {
            _minorityStatuses.RemoveWhere(c => c.Id == id);
        }

        public ProfileMinorityStatus FindMinorityStatusById(long id)
        {
            try
            {
                return _minorityStatuses
                    .Where(c => c.Id == id)
                    .Single();
            }
            catch (Exception ex)
            {
                throw new NotFoundException(nameof(ProfileMinorityStatus), id, ex);
            }
        }

        public void AddProduct(ProfileProduct product)
        {
            _products.Add(product);
        }

        public void RemoveProduct(long id)
        {
            _products.RemoveWhere(c => c.Id == id);
        }

        public ProfileProduct FindProductById(long id)
        {
            try
            {
                return _products
                    .Where(c => c.Id == id)
                    .Single();
            }
            catch (Exception ex)
            {
                throw new NotFoundException(nameof(ProfileProduct), id, ex);
            }
        }

        public void AddContact(ProfileContact contact)
        {
            if (contact.IsMainContact)
            {
                var currentMain = _contacts
                    .Where(c => c.IsMainContact)
                    .SingleOrDefault();

                if (currentMain != null)
                {
                    currentMain.SetIsMainContact(false);
                }
            }

            _contacts.Add(contact);
        }

        public void RemoveContact(long id)
        {
            _contacts.RemoveWhere(c => c.Id == id);
        }

        public ProfileContact FindContactById(long id)
        {
            try
            {
                return _contacts
                    .Where(c => c.Id == id)
                    .Single();
            }
            catch (Exception ex)
            {
                throw new NotFoundException(nameof(ProfileContact), id, ex);
            }
        }

#nullable disable
        private Profile() { }
#nullable restore
    }
}
