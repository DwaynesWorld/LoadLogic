using System;
using System.Runtime.Serialization;

namespace LoadLogic.Services.Vendors.Domain
{
    [Serializable]
    public class DuplicateCodeException : Exception
    {
        public DuplicateCodeException(string name, object code)
            : base($"A duplicate record of {name} with code {code} already exists.") { }

        public DuplicateCodeException(string name, object code, Exception inner)
            : base($"A duplicate record of {name} with code {code} already exists.", inner) { }

        protected DuplicateCodeException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}