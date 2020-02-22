using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class DateOutOfRangeException : Exception
    {
        public DateOutOfRangeException()
        {
        }

        public DateOutOfRangeException(string message) : base(message)
        {
        }
    }
}