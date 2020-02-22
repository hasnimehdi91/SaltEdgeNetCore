using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class AispConsentAlreadyRevokedException : Exception
    {
        public AispConsentAlreadyRevokedException()
        {
        }

        public AispConsentAlreadyRevokedException(string message) : base(message)
        {
        }
    }
}