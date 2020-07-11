using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class DateTimeFormatInvalidException : Exception
    {
        public DateTimeFormatInvalidException()
        {
        }

        public DateTimeFormatInvalidException(string message) : base(message)
        {
        }
    }
}