using System;
using System.Runtime.Serialization;

namespace LoadLogic.Services.Exceptions
{
    [Serializable]
    public class DuplicateRecordException : Exception
    {
        public DuplicateRecordException(string name, object key)
            : base($"A duplicate record of {name} with key {key} already exists.") { }

        public DuplicateRecordException(string name, object key, Exception inner)
            : base($"A duplicate record of {name} with key {key} already exists.", inner) { }

        protected DuplicateRecordException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
