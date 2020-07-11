using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class CustomFieldsFormatInvalidException : Exception
    {
        public CustomFieldsFormatInvalidException()
        {
        }

        public CustomFieldsFormatInvalidException(string message) : base(message)
        {
        }
    }
}