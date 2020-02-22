using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class InvalidAispConsentFromDateException : Exception
    {
        public InvalidAispConsentFromDateException()
        {
        }

        public InvalidAispConsentFromDateException(string message) : base(message)
        {
        }
    }
}