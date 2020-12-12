using ChatApp.Identity.Core.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ChatApp.Identity.Core.Repositories
{
    public interface IChatRoomRepository : IBaseRepository<ChatRoomModel>
    {
        Task<int> AddUserToRoomAsync(IEnumerable<string> userIds, string roomId, CancellationToken cancellationToken);
        IEnumerable<ChatUserModel> GetRoomParticipants(string roomId);
        IEnumerable<ChatUserModel> GetNonRoomParticipants(string roomId);
        IEnumerable<ChatRoomModel> GetChatRoomsByUserId(string userId);
    }
}
