using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class ProviderNotFoundException : Exception
    {
        public ProviderNotFoundException()
        {
        }

        public ProviderNotFoundException(string message) : base(message)
        {
        }
    }
}