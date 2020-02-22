using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class AispConsentScopesInvalidException : Exception
    {
        public AispConsentScopesInvalidException()
        {
        }

        public AispConsentScopesInvalidException(string message) : base(message)
        {
        }
    }
}