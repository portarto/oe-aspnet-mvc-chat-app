using ChatApp.Identity.Core.Assets;
using ChatApp.Identity.Core.Models;
using ChatApp.Identity.Core.Repositories;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChatApp.Identity.Managers
{
    public class MessageManager : BaseManager<MessageModel, IMessageRepository>
    {
        protected IIdentityRepository IdentityRepository { get; }

        public MessageManager(
            IMessageRepository messageRepository,
            IIdentityRepository identityRepository,
            ICancellationTokenWrapper cancellationTokenWrapper
        ) : base(messageRepository, cancellationTokenWrapper)
        {
            IdentityRepository = identityRepository;
        }

        public IEnumerable<MessageModel> GetAllByRoomId(string roomId)
            => BaseRepository.GetAllByRoomId(roomId);

        public async Task<int> SaveAsync(string roomId, MessageModel messageModel, ClaimsPrincipal claimsPrincipal)
        {
            var user = await IdentityRepository.GetUserAsync(claimsPrincipal);
            messageModel.SentByUserId = user.Id;
            messageModel.SentToChatRoomId = roomId;
            return await base.SaveAsync(messageModel);
        }
    }
}
