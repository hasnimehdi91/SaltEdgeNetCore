using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class PublicKeyNotProvidedException : Exception
    {
        public PublicKeyNotProvidedException()
        {
        }

        public PublicKeyNotProvidedException(string message) : base(message)
        {
        }
    }
}