using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class FetchScopesInvalidException : Exception
    {
        public FetchScopesInvalidException()
        {
        }

        public FetchScopesInvalidException(string message) : base(message)
        {
        }
    }
}