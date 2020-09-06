using System;

namespace LoadLogic.Services.Vendors.Application.Models
{
    public class ContactDto
    {

        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long CompanyId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string FaxNumber { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string CellPhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string EmailAddress { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public bool IsMainContact { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Note { get; set; } = string.Empty;
    }
}
