using System;

namespace MessageQueue.Sdk
{
    public class SqlResultException : Exception
    {
        public SqlResultException(string message)
            : base(message)
        {
        }
    }
}