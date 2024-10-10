using System.Runtime.Serialization;

namespace RouteRequirementsBL.Exceptions
{
    [Serializable]
    internal class LocationException : Exception
    {
        public LocationException(string? message) : base(message)
        {
        }

        public LocationException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}