using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class AispConsentNotFoundException : Exception
    {
        public AispConsentNotFoundException()
        {
        }

        public AispConsentNotFoundException(string message) : base(message)
        {
        }
    }
}