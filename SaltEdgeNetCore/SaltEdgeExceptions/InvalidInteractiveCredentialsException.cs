using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class InvalidInteractiveCredentialsException : Exception
    {
        public InvalidInteractiveCredentialsException()
        {
        }

        public InvalidInteractiveCredentialsException(string message) : base(message)
        {
        }
    }
}