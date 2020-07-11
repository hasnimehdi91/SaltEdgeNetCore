using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class RequestExpiredException : Exception
    {
        public RequestExpiredException()
        {
        }

        public RequestExpiredException(string message) : base(message)
        {
        }
    }
}