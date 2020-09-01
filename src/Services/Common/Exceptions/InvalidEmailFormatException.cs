using System;
using System.Runtime.Serialization;

namespace LoadLogic.Services.Common
{
    [Serializable]
    public class InvalidEmailFormatException : Exception
    {
        public InvalidEmailFormatException(string email)
            : base($"Email address {email} is invalid.") { }

        public InvalidEmailFormatException(string email, Exception inner)
            : base($"Email address {email} is invalid.", inner) { }

        protected InvalidEmailFormatException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
