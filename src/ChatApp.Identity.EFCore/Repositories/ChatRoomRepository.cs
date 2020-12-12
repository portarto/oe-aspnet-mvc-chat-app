using ChatApp.Identity.Core.Models;
using ChatApp.Identity.Core.Repositories;
using ChatApp.Identity.EFCore.DbContexts;
using ChatApp.Identity.EFCore.Entities;
using ChatApp.Identity.EFCore.Mapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChatApp.Identity.EFCore.Repositories
{
    internal class ChatRoomRepository : BaseRepository<ChatRoomModel, ChatRoom, ChatRoomMapper>, IChatRoomRepository
    {
        protected UserMapper UserMapper { get; }

        public ChatRoomRepository(
            ChatAppDbContext context,
            ChatRoomMapper mapper,
            UserMapper userMapper
        ) : base(context, mapper)
        {
            UserMapper = userMapper;
        }

        public Task<int> AddUserToRoomAsync(IEnumerable<string> userIds, string roomId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var room = GetRoomWithParticipantsById(roomId);

            if (room.ChatRoomParticipants is null)
            {
                room.ChatRoomParticipants = new List<ChatRoomParticipant>();
            }

            foreach (var userId in userIds)
            {
                room.ChatRoomParticipants.Add(new ChatRoomParticipant() { UserId = userId, ChatRoomId = roomId });
            }
            return Context.SaveChangesAsync(cancellationToken);
        }

        public IEnumerable<ChatRoomModel> GetChatRoomsByUserId(string userId)
            => Context
                .ChatRoomParticipants
                .Include(crp => crp.ChatRoom)
                .Where(crp => crp.UserId == userId)
                .Select(crp => Mapper.MapToModel(crp.ChatRoom))
        ;

        public IEnumerable<ChatUserModel> GetRoomParticipants(string roomId)
        {
            var room = GetRoomWithParticipantsById(roomId);
            return room.ChatRoomParticipants?.Select(crp => UserMapper.MapToModel(crp.User));
        }

        public IEnumerable<ChatUserModel> GetNonRoomParticipants(string roomId)
        {
            var nonParticipants = GetNonRoomParticipantsById(roomId);
            return nonParticipants.Select(user => UserMapper.MapToModel(user));
        }

        private IQueryable<ChatUser> GetNonRoomParticipantsById(string roomId)
            => Context
                .Users
                .Include(u => u.ChatRoomParticipants)
                .Where(u => !u.ChatRoomParticipants.Any(crp => crp.ChatRoomId == roomId))
        ;

        private ChatRoom GetRoomWithParticipantsById(string roomId)
            => Context
                .ChatRooms
                .Include(cr => cr.ChatRoomParticipants)
                .ThenInclude(crp => crp.User)
                .First(cr => cr.Id == roomId)
        ;
    }
}
