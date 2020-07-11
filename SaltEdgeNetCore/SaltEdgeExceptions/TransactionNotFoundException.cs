using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class TransactionNotFoundException : Exception
    {
        public TransactionNotFoundException()
        {
        }

        public TransactionNotFoundException(string message) : base(message)
        {
        }
    }
}