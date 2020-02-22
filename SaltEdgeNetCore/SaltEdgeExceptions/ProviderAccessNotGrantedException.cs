using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class ProviderAccessNotGrantedException : Exception
    {
        public ProviderAccessNotGrantedException()
        {
        }

        public ProviderAccessNotGrantedException(string message) : base(message)
        {
        }
    }
}