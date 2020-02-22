using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class ProviderKeyFoundException : Exception
    {
        public ProviderKeyFoundException()
        {
        }

        public ProviderKeyFoundException(string message) : base(message)
        {
        }
    }
}