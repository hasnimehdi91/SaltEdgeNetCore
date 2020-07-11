using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class MissingSignatureException : Exception
    {
        public MissingSignatureException()
        {
        }

        public MissingSignatureException(string message) : base(message)
        {
        }
    }
}