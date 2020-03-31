using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class PrivateKeyMissingException : Exception
    {
        public PrivateKeyMissingException()
        {
        }

        public PrivateKeyMissingException(string message) : base(message)
        {
        }
    }
}