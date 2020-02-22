using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class ProviderNotInteractiveException : Exception
    {
        public ProviderNotInteractiveException()
        {
        }

        public ProviderNotInteractiveException(string message) : base(message)
        {
        }
    }
}