using System;
using System.Threading.Tasks;

using MessageQueue.Sdk;

namespace MessageQueue.Publisher
{
    internal class MessageProducer : IAsyncDisposable
    {
        private readonly WriterConnection writerConnection;

        public MessageProducer(WriterConnection writerConnection)
        {
            this.writerConnection = writerConnection;
        }

        public async ValueTask DisposeAsync() => await this.writerConnection.DisposeAsync();

        public async Task ProduceMessagesAsync(int maxMessages, TimeSpan waitTime)
        {
            for (int i = 0; i < maxMessages; i++)
            {
                Console.WriteLine($"Publishing message ${i} ...)");
                await writerConnection.PushMessageAsync($"Message #{i}");
                await Task.Delay(millisecondsDelay: (int)waitTime.TotalMilliseconds);
            }

            Console.WriteLine("Done.");
        }
    }
}
