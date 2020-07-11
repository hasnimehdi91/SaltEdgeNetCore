using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class InvalidEncodingException : Exception
    {
        public InvalidEncodingException()
        {
        }

        public InvalidEncodingException(string message) : base(message)
        {
        }
    }
}