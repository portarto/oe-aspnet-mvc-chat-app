using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ChatApp.Identity.EFCore.Entities
{
    internal class ChatUser : IdentityUser, IBaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Details { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTimeOffset RegisteredAt { get; set; }

        public IList<ChatRoom> ChatRooms { get; set; }
        public IList<ChatRoomParticipant> ChatRoomParticipants { get; set; }
    }
}
