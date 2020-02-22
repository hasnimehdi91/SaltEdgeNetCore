using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class DuplicatedCustomerException : Exception
    {
        public DuplicatedCustomerException()
        {
        }

        public DuplicatedCustomerException(string message) : base(message)
        {
        }
    }
}