using ChatApp.Identity.Core.Assets;
using ChatApp.Identity.Core.Models;
using ChatApp.Identity.Core.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChatApp.Identity.Managers
{
    public class ChatRoomManager : BaseManager<ChatRoomModel, IChatRoomRepository>
    {
        protected IIdentityRepository IdentityRepository { get; }

        public ChatRoomManager(
            IChatRoomRepository chatRoomRepository,
            ICancellationTokenWrapper cancellationTokenWrapper,
            IIdentityRepository identityRepository
        ) : base(chatRoomRepository, cancellationTokenWrapper)
        {
            IdentityRepository = identityRepository;
        }

        public async Task<int> SaveAsync(ChatRoomModel model, ClaimsPrincipal claimsPrincipal)
        {
            if(string.IsNullOrEmpty(model.Name))
            {
                throw new ArgumentNullException("Chat room's name cannot be null.");
            }
            if(string.IsNullOrWhiteSpace(model.Name))
            {
                throw new ArgumentException("Chat room cannot be whitepace.");
            }

            var result = await base.SaveAsync(model);
            if (result > 0)
            {
                var user = await IdentityRepository.GetUserAsync(claimsPrincipal);
                return await AddUserToRoomAsync(new List<string>() { user.Id }, model.Id);
            }
            return result;
        }

        public Task<int> AddUserToRoomAsync(IEnumerable<string> userIds, string roomId)
            => BaseRepository.AddUserToRoomAsync(userIds, roomId, CancellationToken);

        public IEnumerable<ChatUserModel> GetRoomParticipants(string roomId)
            => BaseRepository.GetRoomParticipants(roomId);

        public IEnumerable<ChatUserModel> GetNonRoomParticipants(string roomId)
            => BaseRepository.GetNonRoomParticipants(roomId);

        public IEnumerable<ChatRoomModel> GetChatRoomsByUserId(string userId)
            => BaseRepository.GetChatRoomsByUserId(userId);
    }
}
