using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class ProviderDisabledException : Exception
    {
        public ProviderDisabledException()
        {
        }

        public ProviderDisabledException(string message) : base(message)
        {
        }
    }
}