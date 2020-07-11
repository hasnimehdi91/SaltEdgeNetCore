using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class ApiKeyNotFoundException : Exception
    {
        public ApiKeyNotFoundException()
        {
        }

        public ApiKeyNotFoundException(string message) : base(message)
        {
        }
    }
}