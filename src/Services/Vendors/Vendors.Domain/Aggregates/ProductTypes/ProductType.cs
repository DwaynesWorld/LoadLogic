using System;
using LoadLogic.Services.Abstractions;

namespace LoadLogic.Services.Vendors.Domain
{
    public class ProductType : Entity, IAggregateRoot
    {
        public ProductType(string code, string description)
            : this(default, code, description)
        {
        }

        public ProductType(long id, string code, string description)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException("Code cannot be null, empty, or white space.", nameof(code));
            }

            this.Id = id;
            this.Code = code.ToUpper();
            this.Description = description;
        }


        public string Code { get; private set; }
        public string Description { get; private set; }

        public void Update(string code, string description)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException("Code cannot be null, empty, or white space.", nameof(code));
            }

            this.Code = code.ToUpper();
            this.Description = description;
        }

#nullable disable
        private ProductType() { }
#nullable restore
    }
}
