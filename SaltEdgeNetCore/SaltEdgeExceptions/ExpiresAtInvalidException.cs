using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class ExpiresAtInvalidException : Exception
    {
        public ExpiresAtInvalidException()
        {
        }

        public ExpiresAtInvalidException(string message) : base(message)
        {
        }
    }
}