using System.Runtime.Serialization;

namespace RouteRequirementsBL.Exceptions
{
    [Serializable]
    internal class LocatieException : Exception
    {
        public LocatieException(string? message) : base(message)
        {
        }

        public LocatieException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}