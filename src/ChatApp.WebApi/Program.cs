using ChatApp.Core.StartupTasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            await ExecuteStartupTasks(host);

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static async Task ExecuteStartupTasks(IHost host)
        {
            var serviceScopeFactory = host.Services.GetRequiredService<IServiceScopeFactory>();
            using var scope = serviceScopeFactory.CreateScope();

            var services = scope.ServiceProvider.GetServices<IAsyncStartupTask>().OrderBy(task => task.Order);
            foreach (var service in services)
            {
                await service.ExecuteAsync();
            }
        }
    }
}
