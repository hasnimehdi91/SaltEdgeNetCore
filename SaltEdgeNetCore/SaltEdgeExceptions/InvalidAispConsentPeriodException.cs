using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class InvalidAispConsentPeriodException : Exception
    {
        public InvalidAispConsentPeriodException()
        {
        }

        public InvalidAispConsentPeriodException(string message) : base(message)
        {
        }
    }
}