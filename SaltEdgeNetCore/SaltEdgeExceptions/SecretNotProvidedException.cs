using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class SecretNotProvidedException : Exception
    {
        public SecretNotProvidedException()
        {
        }

        public SecretNotProvidedException(string message) : base(message)
        {
        }
    }
}