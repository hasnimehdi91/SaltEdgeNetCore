using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class WrongProviderModeException : Exception
    {
        public WrongProviderModeException()
        {
        }

        public WrongProviderModeException(string message) : base(message)
        {
        }
    }
}