using ChatApp.Identity.Managers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApp.Identity
{
    public class IoC
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            ChatApp.Identity.EFCore.IoC.ConfigureServices(services, configuration);

            services.AddScoped<ChatRoomManager>();
            services.AddScoped<MessageManager>();
            services.AddScoped<IdentityManager>();
        }
    }
}
