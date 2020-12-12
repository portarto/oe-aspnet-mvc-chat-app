using ChatApp.Core.Models;
using ChatApp.Core.StartupTasks;
using ChatApp.Identity.Core.Assets;
using ChatApp.WebApi.Assets;
using ChatApp.WebApi.StartupTasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ChatApp.WebApi
{
    public partial class Startup
    {
        public void ConfigureIdentityServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ICancellationTokenWrapper, AspNetCancellationTokenWrapper>();

            services.Configure<JwtConfig>(o => Configuration.GetSection("JwtConfig").Bind(o));
            var jwtConfig = Configuration.GetSection("JwtConfig").Get<JwtConfig>();

            ChatApp.Identity.IoC.ConfigureServices(services, Configuration);

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtConfig.Issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfig.Secret)),
                    ValidAudience = jwtConfig.Audience,
                    ValidateAudience = true,
                    ValidateLifetime = true
                };
            });

            services.AddScoped<IAsyncStartupTask, InitSeedDb>();
        }
    }
}
