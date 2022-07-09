using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

using MessageQueue.Sdk;

namespace MessageQueue.Cleanup
{
    static class Program
    {
        static IServiceCollection ConfigureServices(IServiceCollection services)
        {
            return services
                .AddSingleton(new DurableQueue("test"))
                .AddSingleton<MessageCleanup>();
        }

        static async Task<int> Main()
        {
            try
            {
                var serviceProvider = ConfigureServices(new ServiceCollection()).BuildServiceProvider();
                var messageConsumer = serviceProvider.GetRequiredService<MessageCleanup>();
                await messageConsumer.DeleteMessagesAsync();

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
