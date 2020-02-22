using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class InvalidToDateException : Exception
    {
        public InvalidToDateException()
        {
        }

        public InvalidToDateException(string message) : base(message)
        {
        }
    }
}