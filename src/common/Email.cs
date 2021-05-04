using System;
using System.Collections.Generic;
using LoadLogic.Services.Exceptions;

namespace LoadLogic.Services
{
    public class Email : ValueObject
    {
        public string Identifier { get; private set; } = string.Empty;
        public string Domain { get; private set; } = string.Empty;

        public Email()
        {
        }

        public Email(string value, bool checkFormat = true)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                try
                {
                    var index = value.IndexOf("@", StringComparison.Ordinal);
                    Identifier = value.Substring(0, index);
                    Domain = value[(index + 1)..];
                }
                catch (Exception ex)
                {
                    if (checkFormat) throw new InvalidEmailFormatException(value, ex);
                }
            }
        }

        public static implicit operator string(Email email)
        {
            return email.ToString();
        }

        public static explicit operator Email(string value)
        {
            return new Email(value);
        }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Identifier) || string.IsNullOrWhiteSpace(Domain))
            {
                return string.Empty;
            }

            return $"{Identifier}@{Domain}";
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Identifier;
            yield return Domain;
        }
    }
}
