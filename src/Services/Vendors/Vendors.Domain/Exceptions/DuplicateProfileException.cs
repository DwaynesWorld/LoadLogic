using System;
using System.Runtime.Serialization;

namespace LoadLogic.Services.Vendors.Domain
{
    [Serializable]
    public class DuplicateProfileException : Exception
    {
        public DuplicateProfileException(object key)
            : base($"A profile for company with {key} already exists.") { }

        public DuplicateProfileException(object key, Exception inner)
            : base($"A profile for company with {key} already exists.", inner) { }

        protected DuplicateProfileException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}