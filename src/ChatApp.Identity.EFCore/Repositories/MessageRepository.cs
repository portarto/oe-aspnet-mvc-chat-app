using ChatApp.Identity.Core.Models;
using ChatApp.Identity.Core.Repositories;
using ChatApp.Identity.EFCore.DbContexts;
using ChatApp.Identity.EFCore.Entities;
using ChatApp.Identity.EFCore.Mapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ChatApp.Identity.EFCore.Repositories
{
    internal class MessageRepository : BaseRepository<MessageModel, Message, MessageMapper>, IMessageRepository
    {
        public MessageRepository(
            ChatAppDbContext context,
            MessageMapper mapper
        ) : base(context, mapper)
        { }

        public IEnumerable<MessageModel> GetAllByRoomId(string roomId)
            => Context
                .Messages
                .Include(m => m.User)
                .Where(e => e.SentToChatRoomId == roomId)
                .OrderBy(e => e.SentAt)
                .Select(e => Mapper.MapToModel(e))
                .AsEnumerable()
        ;
    }
}
