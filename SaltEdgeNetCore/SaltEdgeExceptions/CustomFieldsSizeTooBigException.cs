using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class CustomFieldsSizeTooBigException : Exception
    {
        public CustomFieldsSizeTooBigException()
        {
        }

        public CustomFieldsSizeTooBigException(string message) : base(message)
        {
        }
    }
}