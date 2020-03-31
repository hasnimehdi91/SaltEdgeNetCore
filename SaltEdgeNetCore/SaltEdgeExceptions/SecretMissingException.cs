using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class SecretMissingException : Exception
    {
        public SecretMissingException()
        {
        }

        public SecretMissingException(string message) : base(message)
        {
        }
    }
}