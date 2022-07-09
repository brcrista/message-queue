using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

using MessageQueue.Sdk;

namespace MessageQueue.Subscriber
{
    static class Program
    {
        static IServiceCollection ConfigureServices(IServiceCollection services)
        {
            return services
                .AddSingleton(new ReaderConnection("test"))
                .AddSingleton<MessageConsumer>();
        }

        static async Task<int> Main()
        {
            try
            {
                var serviceProvider = ConfigureServices(new ServiceCollection()).BuildServiceProvider();
                var messageConsumer = serviceProvider.GetRequiredService<MessageConsumer>();
                await messageConsumer.ConsumeMessagesAsync();

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
