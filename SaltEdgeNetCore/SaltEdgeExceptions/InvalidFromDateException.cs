using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class InvalidFromDateException : Exception
    {
        public InvalidFromDateException()
        {
        }

        public InvalidFromDateException(string message) : base(message)
        {
        }
    }
}