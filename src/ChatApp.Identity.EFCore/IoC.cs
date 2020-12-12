using ChatApp.Core.StartupTasks;
using ChatApp.Identity.Core.Models;
using ChatApp.Identity.Core.Repositories;
using ChatApp.Identity.EFCore.DbContexts;
using ChatApp.Identity.EFCore.Entities;
using ChatApp.Identity.EFCore.Mapper;
using ChatApp.Identity.EFCore.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApp.Identity.EFCore
{
    public static class IoC
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ChatAppDbContext>(
                   options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
               );

            services.AddScoped<IAsyncStartupTask, MigrateDatabase>();

            services.AddIdentity<ChatUser, IdentityRole>(
                    option =>
                    {
                        option.Password.RequireDigit = false;
                        option.Password.RequiredLength = 6;
                        option.Password.RequireNonAlphanumeric = false;
                        option.Password.RequireUppercase = false;
                        option.Password.RequireLowercase = false;
                    }
                ).AddEntityFrameworkStores<ChatAppDbContext>()
                .AddDefaultTokenProviders()
            ;

            services.AddScoped<ChatRoomMapper>();
            services.AddScoped<MessageMapper>();
            services.AddScoped<UserMapper>();

            services.AddScoped<IChatRoomRepository, ChatRoomRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IIdentityRepository, IdentityRepository>();
        }
    }
}
