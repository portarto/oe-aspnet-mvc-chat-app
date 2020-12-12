using ChatApp.Core.StartupTasks;
using ChatApp.Identity.Core.Models;
using ChatApp.Identity.Managers;
using ChatApp.WebApi.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.WebApi.StartupTasks
{
    public class InitSeedDb : IAsyncStartupTask
    {
        protected IdentityManager IdentityManager { get; }
        
        public int Order => 5;


        public InitSeedDb(IdentityManager identityManager)
        {
            IdentityManager = identityManager;
        }

        public async Task ExecuteAsync()
        {
            for (int idx = 0; idx < 15; idx++)
            {
                await IdentityManager.RegisterUser(GetMigrationModel(idx));
            }
        }

        private RegisterModel GetMigrationModel(int idx)
            => new RegisterModel()
            {
                Email = $"test-{idx}@test.test",
                Username = $"test-{idx}",
                FirstName = $"Test_{idx}",
                LastName = "User",
                Password = "asdasd",
                DateOfBirth = new DateTime(1985, 1, 1)
            }
        ;
    }
}
