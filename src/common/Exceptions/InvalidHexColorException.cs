using System;
using System.Runtime.Serialization;

namespace LoadLogic.Services.Exceptions
{
    [Serializable]
    public class InvalidHexColorException : Exception
    {
        public InvalidHexColorException(string color)
            : base($"Hex value {color} is not a valid hex color.") { }

        public InvalidHexColorException(string color, Exception inner)
            : base($"Hex value {color} is not a valid hex color.", inner) { }

        protected InvalidHexColorException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
