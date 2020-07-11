using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class JsonParseErrorException : Exception
    {
        public JsonParseErrorException()
        {
        }

        public JsonParseErrorException(string message) : base(message)
        {
        }
    }
}