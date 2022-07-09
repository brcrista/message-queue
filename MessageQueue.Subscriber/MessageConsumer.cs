using System;
using System.Threading.Tasks;

using MessageQueue.Sdk;

namespace MessageQueue.Subscriber
{
    internal class MessageConsumer : IAsyncDisposable
    {
        private readonly DurableQueue queue;

        public MessageConsumer(DurableQueue queue)
        {
            this.queue = queue;
        }

        public async ValueTask DisposeAsync() => await this.queue.DisposeAsync();

        public async Task ConsumeMessagesAsync()
        {
            int messagesReceived = 0;
            while (true)
            {
                var message = await this.queue.PullAsync();
                if (message is null)
                {
                    break;
                }

                messagesReceived++;
                Console.WriteLine($"Received message '{message}'");
            }

            Console.WriteLine($"Messages received: {messagesReceived}.");
        }
    }
}
