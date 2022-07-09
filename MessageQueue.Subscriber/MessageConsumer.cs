using System;
using System.Threading.Tasks;

using MessageQueue.Sdk;

namespace MessageQueue.Subscriber
{
    internal class MessageConsumer : IAsyncDisposable
    {
        private readonly ReaderConnection readerConnection;

        public MessageConsumer(ReaderConnection readerConnection)
        {
            this.readerConnection = readerConnection;
        }

        public async ValueTask DisposeAsync() => await this.readerConnection.DisposeAsync();

        public async Task ConsumeMessagesAsync()
        {
            while (true)
            {
                var message = await readerConnection.PullMessageAsync();
                if (message is null)
                {
                    break;
                }

                Console.WriteLine($"Received message '{message}'");
            }

            Console.WriteLine("Done.");
        }
    }
}
