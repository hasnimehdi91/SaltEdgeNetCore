using System;

namespace SaltEdgeNetCore.SaltEdgeExceptions
{
    public class ActionNotAllowedException : Exception
    {
        public ActionNotAllowedException()
        {
        }

        public ActionNotAllowedException(string message) : base(message)
        {
        }
    }
}