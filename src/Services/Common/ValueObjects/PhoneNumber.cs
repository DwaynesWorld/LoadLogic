using System.Collections.Generic;

namespace LoadLogic.Services.Common
{
    // TODO: Add Phone Number specfic logic
    public class PhoneNumber : ValueObject
    {
        public string Number { get; private set; } = string.Empty;

        public PhoneNumber()
        {
        }

        public PhoneNumber(string number)
        {
            this.Number = number;
        }

        public static implicit operator string(PhoneNumber phoneNumber)
        {
            return phoneNumber.ToString();
        }

        public static explicit operator PhoneNumber(string value)
        {
            return new PhoneNumber(value);
        }

        public override string ToString()
        {
            return Number ?? "";
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Number;
        }
    }
}
