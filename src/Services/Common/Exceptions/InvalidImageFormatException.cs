using System;
using System.Runtime.Serialization;

namespace LoadLogic.Services.Exceptions
{
    [Serializable]
    public class InvalidImageFormatException : Exception
    {
        public InvalidImageFormatException(string contentType)
            : base($"Content type {contentType} is not an accepted image format or could not be validated.") { }

        public InvalidImageFormatException(string contentType, Exception inner)
            : base($"Content type {contentType} is not an accepted image format or could not be validated.", inner) { }

        protected InvalidImageFormatException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
