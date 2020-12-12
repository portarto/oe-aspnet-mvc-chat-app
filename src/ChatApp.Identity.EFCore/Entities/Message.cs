using Microsoft.AspNetCore.Identity;
using System;

namespace ChatApp.Identity.EFCore.Entities
{
    internal class Message : IBaseEntity
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string SentByUserId { get; set; }
        public string SentToChatRoomId { get; set; }
        public DateTime SentAt { get; set; }

        public ChatRoom ChatRoom { get; set; }
        public ChatUser User { get; set; }
    }
}
