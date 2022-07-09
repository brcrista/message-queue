using System;
using System.Threading.Tasks;

using MessageQueue.Sdk;

namespace MessageQueue.Publisher
{
    internal class MessageProducer : IAsyncDisposable
    {
        private readonly DurableQueue queue;

        public MessageProducer(DurableQueue queue)
        {
            this.queue = queue;
        }

        public async ValueTask DisposeAsync() => await this.queue.DisposeAsync();

        public async Task ProduceMessagesAsync(int maxMessages, TimeSpan waitTime)
        {
            await queue.InitializeAsync();
            for (int i = 0; i < maxMessages; i++)
            {
                Console.WriteLine($"Publishing message {i} ...");
                await this.queue.PushAsync($"Message #{i}");
                await Task.Delay(millisecondsDelay: (int)waitTime.TotalMilliseconds);
            }

            Console.WriteLine("Done.");
        }
    }
}
