using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class SignatureNotMatchException : Exception
    {
        public SignatureNotMatchException()
        {
        }

        public SignatureNotMatchException(string message) : base(message)
        {
        }
    }
}