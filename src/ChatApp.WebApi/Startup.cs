using ChatApp.WebApi.Chat.Hubs;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using NJsonSchema.Generation;
using System.Text.Json;

namespace ChatApp.WebApi
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("EnableAllCors", builder =>
                builder.WithOrigins("http://localhost:4200", "https://localhost:5001")
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials()
            ));

            services
                .AddSignalR()
                .AddHubOptions<MessageHub>(o => o.EnableDetailedErrors = true)
                .AddJsonProtocol(
                    o =>
                    {
                        o.PayloadSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    }
                )
            ;

            services
                .AddSignalR()
                .AddHubOptions<RoomHub>(o => o.EnableDetailedErrors = true)
                .AddJsonProtocol(
                    o =>
                    {
                        o.PayloadSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    }
                )
            ;

            services.AddOpenApiDocument(settings =>
            {
                settings.SerializerSettings = new Newtonsoft.Json.JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
            });

            services
                .AddControllers()
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                })
            ;

            ConfigureIdentityServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseOpenApi();
                app.UseSwaggerUi3();
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ChatApp.WebApi v1"));
            }

            app.UseRouting();
            app.UseCors("EnableAllCors");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<MessageHub>("/hub/messages");
                endpoints.MapHub<RoomHub>("/hub/rooms");
            });

            app.Map("/hub/messages/negotiate", map =>
            {
                map.UseCors("EnableAllCors");
                var hubConfiguration = new HubConfiguration
                {
                    EnableDetailedErrors = true
                };
            });
        }
    }
}
