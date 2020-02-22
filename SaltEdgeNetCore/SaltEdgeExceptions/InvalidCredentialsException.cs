using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException()
        {
        }

        public InvalidCredentialsException(string message) : base(message)
        {
        }
    }
}