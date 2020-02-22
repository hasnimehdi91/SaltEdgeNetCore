using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class ReturnURLTooLongException : Exception
    {
        public ReturnURLTooLongException()
        {
        }

        public ReturnURLTooLongException(string message) : base(message)
        {
        }
    }
}