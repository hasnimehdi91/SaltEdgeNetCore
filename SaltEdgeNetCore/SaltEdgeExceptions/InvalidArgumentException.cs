using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class InvalidArgumentException : Exception
    {
        public InvalidArgumentException()
        {
        }

        public InvalidArgumentException(string message) : base(message)
        {
        }
    }
}