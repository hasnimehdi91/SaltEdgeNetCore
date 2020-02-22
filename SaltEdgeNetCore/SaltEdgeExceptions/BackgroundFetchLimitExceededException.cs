using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class BackgroundFetchLimitExceededException : Exception
    {
        public BackgroundFetchLimitExceededException()
        {
        }

        public BackgroundFetchLimitExceededException(string message) : base(message)
        {
        }
    }
}