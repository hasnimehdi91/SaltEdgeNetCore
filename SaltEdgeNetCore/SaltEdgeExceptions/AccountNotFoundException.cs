using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class AccountNotFoundException : Exception
    {
        public AccountNotFoundException()
        {
        }

        public AccountNotFoundException(string message) : base(message)
        {
        }
    }
}