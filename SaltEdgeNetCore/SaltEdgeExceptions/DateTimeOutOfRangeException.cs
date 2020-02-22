using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class DateTimeOutOfRangeException : Exception
    {
        public DateTimeOutOfRangeException()
        {
        }

        public DateTimeOutOfRangeException(string message) : base(message)
        {
        }
    }
}