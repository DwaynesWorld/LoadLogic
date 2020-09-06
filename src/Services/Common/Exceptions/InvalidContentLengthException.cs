using System;
using System.Runtime.Serialization;

namespace LoadLogic.Services.Exceptions
{
    [Serializable]
    public class InvalidContentLengthException : Exception
    {
        public InvalidContentLengthException(long contentLength, long maxContentLength)
            : base($"Content length {contentLength} is greater than the maximum content length of {maxContentLength}.") { }

        public InvalidContentLengthException(long contentLength, long maxContentLength, Exception inner)
            : base($"Content length {contentLength} is greater than the maximum content length of {maxContentLength}.", inner) { }

        protected InvalidContentLengthException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
