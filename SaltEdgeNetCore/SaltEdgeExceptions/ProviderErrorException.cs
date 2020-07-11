using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class ProviderErrorException : Exception
    {
        public ProviderErrorException()
        {
        }

        public ProviderErrorException(string message) : base(message)
        {
        }
    }
}