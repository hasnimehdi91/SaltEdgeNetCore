using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class EmailInvalidException : Exception
    {
        public EmailInvalidException()
        {
        }

        public EmailInvalidException(string message) : base(message)
        {
        }
    }
}