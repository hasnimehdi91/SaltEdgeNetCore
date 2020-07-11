using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class AppIdMissingException : Exception
    {
        public AppIdMissingException()
        {
        }

        public AppIdMissingException(string message) : base(message)
        {
        }
    }
}