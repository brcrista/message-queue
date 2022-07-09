using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

using MessageQueue.Sdk;

namespace MessageQueue.Publisher
{
    static class Program
    {
        static IServiceCollection ConfigureServices(IServiceCollection services)
        {
            return services
                .AddSingleton(new WriterConnection("test"))
                .AddSingleton<MessageProducer>();
        }

        static async Task<int> Main()
        {
            try
            {
                var serviceProvider = ConfigureServices(new ServiceCollection()).BuildServiceProvider();
                var messageProducer = serviceProvider.GetRequiredService<MessageProducer>();
                await messageProducer.ProduceMessagesAsync(maxMessages: 1024, waitTime: TimeSpan.FromSeconds(1));

                return 0;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                return 1;
            }

        }
    }
}
