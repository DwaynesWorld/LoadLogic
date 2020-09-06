using System;
using System.Collections.Generic;
using System.Linq;
using LoadLogic.Services.Abstractions;
using LoadLogic.Services.Exceptions;

namespace LoadLogic.Services.Vendors.Domain
{
    public partial class Vendor : Company, IAggregateRoot
    {
        private readonly HashSet<VendorMinorityStatus> _minorityStatuses;
        private readonly HashSet<VendorContact> _contacts;
        private readonly HashSet<VendorProduct> _products;

        public Vendor(
            string code, string name, CompanyType? type,
            Address? primaryAddress, Address? alternateAddress, PhoneNumber? phoneNumber, PhoneNumber? faxNumber,
            string webAddress, Region? region, CommunicationMethod communicationMethod,
            bool isBonded, decimal bondRate, string note)
            : this(
                default, code, name, type, primaryAddress,
                alternateAddress, phoneNumber, faxNumber,
                webAddress, region, communicationMethod, isBonded, bondRate, note)
        {
        }

        public Vendor(
            long id, string code, string name, CompanyType? type,
            Address? primaryAddress, Address? alternateAddress, PhoneNumber? phoneNumber, PhoneNumber? faxNumber,
            string webAddress, Region? region, CommunicationMethod communicationMethod,
            bool isBonded, decimal bondRate, string note)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException("Vendor code is required.", nameof(code));
            }


            this.Id = id;
            this.Type = type;
            this.Code = code.ToUpper();
            this.Name = name;
            this.PrimaryAddress = primaryAddress;
            this.AlternateAddress = alternateAddress;
            this.PhoneNumber = phoneNumber;
            this.FaxNumber = faxNumber;
            this.WebAddress = webAddress;
            this.Region = region;
            this.CommunicationMethod = communicationMethod;
            this.IsBonded = isBonded;
            this.BondRate = bondRate;
            this.Note = note;

            _minorityStatuses = new HashSet<VendorMinorityStatus>();
            _contacts = new HashSet<VendorContact>();
            _products = new HashSet<VendorProduct>();
        }

        public string Code { get; private set; }
        public long? TypeId { get; private set; }
        public CompanyType? Type { get; private set; }
        public bool IsBonded { get; private set; }
        public decimal BondRate { get; private set; }
        public string Note { get; private set; }
        public IReadOnlyCollection<VendorMinorityStatus> MinorityStatuses => _minorityStatuses;
        public IReadOnlyCollection<VendorContact> Contacts => _contacts;
        public IReadOnlyCollection<VendorProduct> Products => _products;

        public void Update(
            string code, string name, CompanyType? type,
            Address? primaryAddress, Address? alternateAddress,
            PhoneNumber? phoneNumber, PhoneNumber? faxNumber,
            string webAddress, Region? region, CommunicationMethod communicationMethod,
            bool isBonded, decimal bondRate, string note)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException("Vendor code is required.", nameof(code));
            }

            this.Type = type;
            this.Code = code.ToUpper();
            this.Name = name;
            this.PrimaryAddress = primaryAddress;
            this.AlternateAddress = alternateAddress;
            this.PhoneNumber = phoneNumber;
            this.FaxNumber = faxNumber;
            this.WebAddress = webAddress;
            this.Region = region;
            this.CommunicationMethod = communicationMethod;
            this.IsBonded = isBonded;
            this.BondRate = bondRate;
            this.Note = note;
        }

        public void AddMinorityStatus(VendorMinorityStatus status)
        {
            _minorityStatuses.Add(status);
        }

        public void RemoveMinorityStatus(long id)
        {
            _minorityStatuses.RemoveWhere(c => c.Id == id);
        }

        public VendorMinorityStatus FindMinorityStatusById(long id)
        {
            try
            {
                return _minorityStatuses
                    .Where(c => c.Id == id)
                    .Single();
            }
            catch (System.Exception)
            {
                throw new NotFoundException(nameof(VendorMinorityStatus), id);
            }
        }

        public void AddProduct(VendorProduct product)
        {
            _products.Add(product);
        }

        public void RemoveProduct(long id)
        {
            _products.RemoveWhere(c => c.Id == id);
        }

        public VendorProduct FindProductById(long id)
        {
            try
            {
                return _products
                    .Where(c => c.Id == id)
                    .Single();
            }
            catch (System.Exception)
            {
                throw new NotFoundException(nameof(VendorProduct), id);
            }
        }

        public void AddContact(VendorContact contact)
        {
            if (contact.IsMainContact) ClearCurrentMainContact();
            _contacts.Add(contact);
        }

        public void RemoveContact(long id)
        {
            _contacts.RemoveWhere(c => c.Id == id);
        }

        public void ClearCurrentMainContact()
        {
            var currentMain = _contacts
                .Where(c => c.IsMainContact)
                .SingleOrDefault();

            if (currentMain != null)
            {
                currentMain.SetIsMainContact(false);
            }
        }

        public VendorContact FindContactById(long id)
        {
            try
            {
                return _contacts
                    .Where(c => c.Id == id)
                    .Single();
            }
            catch (System.Exception)
            {
                throw new NotFoundException(nameof(VendorContact), id);
            }
        }

#nullable disable
        private Vendor() { }
#nullable restore
    }
}
