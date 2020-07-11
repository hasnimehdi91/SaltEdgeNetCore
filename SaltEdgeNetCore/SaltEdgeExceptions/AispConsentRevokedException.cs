using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class AispConsentRevokedException : Exception
    {
        public AispConsentRevokedException()
        {
        }

        public AispConsentRevokedException(string message) : base(message)
        {
        }
    }
}