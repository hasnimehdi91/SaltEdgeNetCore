using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class CredentialsNotMatchException : Exception
    {
        public CredentialsNotMatchException()
        {
        }

        public CredentialsNotMatchException(string message) : base(message)
        {
        }
    }
}