using System;
using System.Threading.Tasks;

using MessageQueue.Sdk;

namespace MessageQueue.Cleanup
{
    internal class MessageCleanup : IAsyncDisposable
    {
        private readonly DurableQueue queue;

        public MessageCleanup(DurableQueue queue)
        {
            this.queue = queue;
        }

        public async ValueTask DisposeAsync() => await this.queue.DisposeAsync();

        public async Task DeleteMessagesAsync()
        {
            int messagesDeleted = await queue.PruneAsync();
            Console.WriteLine($"Messages deleted: {messagesDeleted}.");
        }
    }
}
