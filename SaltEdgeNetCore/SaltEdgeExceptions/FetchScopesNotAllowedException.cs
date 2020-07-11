using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class FetchScopesNotAllowedException : Exception
    {
        public FetchScopesNotAllowedException()
        {
        }

        public FetchScopesNotAllowedException(string message) : base(message)
        {
        }
    }
}