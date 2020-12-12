using ChatApp.Core.StartupTasks;
using ChatApp.Identity.EFCore.DbContexts;
using ChatApp.Identity.EFCore.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ChatApp.Identity.EFCore
{
    internal class MigrateDatabase : IAsyncStartupTask
    {
        private ChatAppDbContext DbContext { get; }
        private ILogger<MigrateDatabase> Logger { get; }

        public UserManager<ChatUser> UserManager { get; }
        public int Order => 0;

        public MigrateDatabase(
            ChatAppDbContext dbContext,
            ILogger<MigrateDatabase> logger,
            UserManager<ChatUser> userManager
        )
        {
            DbContext = dbContext;
            Logger = logger;
            UserManager = userManager;
        }

        public async Task ExecuteAsync()
        {
            Logger.LogInformation("Executing migration!");

            Logger.LogInformation("Migrating db...");
            await InitMigration();
            Logger.LogInformation("Db migrated");
        }
        
        private Task InitMigration()
            => DbContext.Database.MigrateAsync();
    }
}
