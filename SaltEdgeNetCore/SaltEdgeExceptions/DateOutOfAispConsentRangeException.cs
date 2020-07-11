using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class DateOutOfAispConsentRangeException : Exception
    {
        public DateOutOfAispConsentRangeException()
        {
        }

        public DateOutOfAispConsentRangeException(string message) : base(message)
        {
        }
    }
}