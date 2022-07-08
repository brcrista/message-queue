using System;

namespace MessageQueue.Sdk
{
    public class MessageQueueNotFoundException : Exception
    {
        public MessageQueueNotFoundException(string messageQueueName)
            : base(message: $"The message queue {messageQueueName} does not exist. Create a new {nameof(WriterConnection)} with this name to create the message queue.")
        {
        }
    }

    public class SqlResultException : Exception
    {
        public SqlResultException(string message)
            : base(message)
        {
        }
    }
}