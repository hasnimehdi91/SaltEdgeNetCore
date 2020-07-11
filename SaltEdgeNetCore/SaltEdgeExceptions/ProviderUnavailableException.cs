using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class ProviderUnavailableException : Exception
    {
        public ProviderUnavailableException()
        {
        }

        public ProviderUnavailableException(string message) : base(message)
        {
        }
    }
}