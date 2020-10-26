using System;
using System.Runtime.Serialization;

namespace LoadLogic.Services.Exceptions
{
    [Serializable]
    public class NotFoundException : Exception
    {
        public NotFoundException(string name, object key)
            : base($"Entity {name} with key {key} was not found.") { }

        public NotFoundException(string name, object key, Exception inner)
            : base($"Entity {name} with key {key} was not found.", inner) { }

        protected NotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
