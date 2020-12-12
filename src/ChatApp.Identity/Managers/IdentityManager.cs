using ChatApp.Core.Models;
using ChatApp.Identity.Core.Assets;
using ChatApp.Identity.Core.Models;
using ChatApp.Identity.Core.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatApp.Identity.Managers
{
    public class IdentityManager
    {
        protected JwtConfig JwtConfig { get; }
        protected byte[] Secret { get; }

        public IEnumerable<ChatUserModel> Users => IdentityRepository.GetAll();

        protected CancellationToken CancellationToken { get; }
        public IIdentityRepository IdentityRepository { get; }

        public IdentityManager(
            IIdentityRepository identityRepository,
            IOptions<JwtConfig> jwtConfigOptions,
            ICancellationTokenWrapper cancellationTokenWrapper
        )
        {
            JwtConfig = jwtConfigOptions.Value;
            CancellationToken = cancellationTokenWrapper.CancellationToken;
            IdentityRepository = identityRepository;
            Secret = Encoding.ASCII.GetBytes(JwtConfig.Secret);
        }

        public Task<ChatUserModel> GetUserByUsernameAsync(string username) => IdentityRepository.FindByNameAsync(username);

        public Task<ChatUserModel> GetUserByEmailAsync(string email) => IdentityRepository.FindByEmailAsync(email);

        public ChatUserModel GetById(string id) => IdentityRepository.FindById(id);

        public Task<ChatUserModel> GetUserAsync(ClaimsPrincipal user)
            => IdentityRepository.GetUserAsync(user);

        public async Task<ChatUserModel> UpdateUser(ChatUserModel model, ClaimsPrincipal claimsPrincipal)
        {
            var sentUser = await IdentityRepository.FindByEmailAsync(model.Email);
            if (sentUser is null)
            {
                throw new ArgumentNullException("User does not exist.");
            }

            var loggedInUser = await GetUserAsync(claimsPrincipal);
            if (sentUser.Id != loggedInUser.Id)
            {
                throw new InvalidOperationException("Operation cannot be performed.");
            }

            await IdentityRepository.UpdateAsync(loggedInUser.Id, model, CancellationToken);

            return GetById(loggedInUser.Id);
        }

        public async Task<string> RegisterUser(ChatUserModel model)
        {
            var user = await GetUserByEmailAsync(model.Email);
            if (user is null)
            {
                var result = await IdentityRepository.CreateAsync(model);
                if (result.Succeeded)
                {
                    await IdentityRepository.AddToRoleAsync(model.Email, "Customer");
                    await IdentityRepository.AddClaimAsync(model.Email, new Claim(ClaimTypes.Email, model.Email));
                    await IdentityRepository.AddClaimAsync(model.Email, new Claim(JwtRegisteredClaimNames.Sub, model.Email));
                }
            }
            return model.Email;
        }

        public async Task<(OperationResult, JwtSecurityToken)> GetToken(string email, string password)
        {
            if (await IdentityRepository.CheckPasswordAsync(email, password))
            {
                var user = await IdentityRepository.FindByEmailAsync(email);
                var claims = new[] {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id)
                };

                var jwtToken = new JwtSecurityToken
                (
                    issuer: JwtConfig.Issuer,
                    audience: JwtConfig.Audience,
                    claims,
                    expires: DateTime.Now.AddMinutes(JwtConfig.AccessTokenExpiration),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Secret), SecurityAlgorithms.HmacSha256Signature)
                );

                return (OperationResult.Succeeded, jwtToken);
            }
            return (OperationResult.Failed, null);
        }
    }
}
