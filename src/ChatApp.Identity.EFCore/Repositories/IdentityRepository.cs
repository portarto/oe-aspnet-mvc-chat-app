using ChatApp.Identity.Core.Models;
using ChatApp.Identity.Core.Repositories;
using ChatApp.Identity.EFCore.DbContexts;
using ChatApp.Identity.EFCore.Entities;
using ChatApp.Identity.EFCore.Mapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace ChatApp.Identity.EFCore.Repositories
{
    internal class IdentityRepository : BaseRepository<ChatUserModel, ChatUser, UserMapper>, IIdentityRepository
    {
        protected UserManager<ChatUser> UserManager { get; }

        public IdentityRepository(
            ChatAppDbContext context,
            UserMapper mapper,
            UserManager<ChatUser> userManager
        ) : base(context, mapper)
        {
            UserManager = userManager;
        }

        public override IEnumerable<ChatUserModel> GetAll() => UserManager.Users.AsEnumerable().Select(user => Mapper.MapToModel(user));

        public async Task<IdentityResult> AddClaimAsync(string email, Claim claim)
        {
            var user = await UserManager.FindByEmailAsync(email);
            if (user is not null)
            {
                return await UserManager.AddClaimAsync(user, claim);
            }

            return IdentityResult.Failed(new IdentityError() { Description = "User does not exist." });
        }

        public async Task<IdentityResult> AddToRoleAsync(string email, string role)
        {
            var user = await UserManager.FindByEmailAsync(email);
            if (user is not null)
            {
                return await UserManager.AddToRoleAsync(user, role);
            }

            return IdentityResult.Failed(new IdentityError() { Description = "User does not exist." });
        }

        public async Task<bool> CheckPasswordAsync(string email, string password)
        {
            var user = await UserManager.FindByEmailAsync(email);
            return user is null ? false : await UserManager.CheckPasswordAsync(user, password);
        }

        public async Task<IdentityResult> CreateAsync(ChatUserModel userModel)
        {
            if (userModel is null)
            {
                throw new ArgumentNullException(nameof(userModel), "User model cannot be null!");
            }

            var user = Mapper.MapRegisterModelToEntity(userModel);
            return await UserManager.CreateAsync(user, userModel.Password);
        }

        public async Task<ChatUserModel> FindByEmailAsync(string email)
            => Mapper.MapToModel(await UserManager.FindByEmailAsync(email));

        public async Task<ChatUserModel> FindByNameAsync(string username)
            => Mapper.MapToModel(await UserManager.FindByNameAsync(username));

        public async Task<ChatUserModel> GetUserAsync(ClaimsPrincipal user)
        {
            var cio = new ClaimsIdentityOptions();
            var bap = user.FindFirstValue(cio.UserIdClaimType);
            var userEntity = await UserManager.GetUserAsync(user);
            return Mapper.MapToModel(userEntity);
        }

        public override async Task<int> UpdateAsync(string id, ChatUserModel model, CancellationToken cancellationToken)
        {
            var user = await UserManager.FindByIdAsync(id);
            Mapper.MapForUpdate(model, user);
            var result = await UserManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return 1;
            }

            return -1;
        }

        public override async Task<int> DeleteAsync(string id, CancellationToken cancellationToken)
        {
            var user = await UserManager.FindByIdAsync(id);
            var identityResult = await UserManager.DeleteAsync(user);
            return identityResult.Succeeded ? 1 : -1;
        }
    }
}
