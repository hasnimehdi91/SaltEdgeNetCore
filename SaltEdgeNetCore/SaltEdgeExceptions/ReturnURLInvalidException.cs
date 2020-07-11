using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class ReturnURLInvalidException : Exception
    {
        public ReturnURLInvalidException()
        {
        }

        public ReturnURLInvalidException(string message) : base(message)
        {
        }
    }
}