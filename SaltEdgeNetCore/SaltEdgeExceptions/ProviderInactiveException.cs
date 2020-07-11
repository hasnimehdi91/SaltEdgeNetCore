using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class ProviderInactiveException : Exception
    {
        public ProviderInactiveException()
        {
        }

        public ProviderInactiveException(string message) : base(message)
        {
        }
    }
}