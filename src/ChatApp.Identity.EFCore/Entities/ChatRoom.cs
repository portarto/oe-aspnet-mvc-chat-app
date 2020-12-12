using System.Collections.Generic;

namespace ChatApp.Identity.EFCore.Entities
{
    internal class ChatRoom : IBaseEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public ICollection<ChatUser> ChatUsers { get; set; }
        public IList<ChatRoomParticipant> ChatRoomParticipants { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}
