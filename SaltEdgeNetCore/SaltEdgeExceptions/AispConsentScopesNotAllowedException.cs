using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class AispConsentScopesNotAllowedException : Exception
    {
        public AispConsentScopesNotAllowedException()
        {
        }

        public AispConsentScopesNotAllowedException(string message) : base(message)
        {
        }
    }
}