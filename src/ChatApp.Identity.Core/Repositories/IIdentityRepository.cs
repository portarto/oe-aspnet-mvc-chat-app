using ChatApp.Identity.Core.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChatApp.Identity.Core.Repositories
{
    public interface IIdentityRepository : IBaseRepository<ChatUserModel>
    {
        Task<IdentityResult> CreateAsync(ChatUserModel userModel);
        Task<ChatUserModel> FindByNameAsync(string username);
        Task<ChatUserModel> FindByEmailAsync(string email);
        Task<IdentityResult> AddToRoleAsync(string email, string role);
        Task<IdentityResult> AddClaimAsync(string email, Claim claim);
        Task<bool> CheckPasswordAsync(string email, string password);
        Task<ChatUserModel> GetUserAsync(ClaimsPrincipal user);
    }
}
