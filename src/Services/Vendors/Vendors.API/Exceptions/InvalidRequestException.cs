using System;

namespace LoadLogic.Services.Vendors.API
{
    [Serializable]
    public class InvalidRequestException : Exception
    {
        public InvalidRequestException(long pathId, long modelId)
            : base($"Path identifier and model identifier must match. Path: {pathId}, Model: {modelId}") { }
    }
}
