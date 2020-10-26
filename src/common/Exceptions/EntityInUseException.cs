using System;
using System.Runtime.Serialization;

namespace LoadLogic.Services.Exceptions
{
    [Serializable]
    public class EntityDeletionException : Exception
    {
        public EntityDeletionException(string name, object key)
            : base($"Deletion error. The entity {name} with key {key} is currently being used.") { }

        public EntityDeletionException(string name, object key, Exception inner)
            : base($"Deletion error. The entity {name} with key {key} is currently being used.", inner) { }

        protected EntityDeletionException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
