using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class IdentifierInvalidException : Exception
    {
        public IdentifierInvalidException()
        {
        }

        public IdentifierInvalidException(string message) : base(message)
        {
        }
    }
}