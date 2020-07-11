using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class MissingExpiresAtException : Exception
    {
        public MissingExpiresAtException()
        {
        }

        public MissingExpiresAtException(string message) : base(message)
        {
        }
    }
}