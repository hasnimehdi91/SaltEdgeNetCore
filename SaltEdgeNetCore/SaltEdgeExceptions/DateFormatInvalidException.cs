using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class DateFormatInvalidException : Exception
    {
        public DateFormatInvalidException()
        {
        }

        public DateFormatInvalidException(string message) : base(message)
        {
        }
    }
}