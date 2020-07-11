using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class AispConsentExpiredException : Exception
    {
        public AispConsentExpiredException()
        {
        }

        public AispConsentExpiredException(string message) : base(message)
        {
        }
    }
}