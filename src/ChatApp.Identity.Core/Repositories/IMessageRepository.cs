using ChatApp.Identity.Core.Models;
using System.Collections.Generic;

namespace ChatApp.Identity.Core.Repositories
{
    public interface IMessageRepository : IBaseRepository<MessageModel>
    {
        IEnumerable<MessageModel> GetAllByRoomId(string roomId);
    }
}
