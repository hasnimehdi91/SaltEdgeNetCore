using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class AllAccountsExcludedException : Exception
    {
        public AllAccountsExcludedException()
        {
        }

        public AllAccountsExcludedException(string message) : base(message)
        {
        }
    }
}