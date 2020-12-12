using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Identity.EFCore.Entities
{
    internal class ChatRoomParticipant
    {
        public string UserId { get; set; }
        public string ChatRoomId { get; set; }

        public ChatUser User { get; set; }
        public ChatRoom ChatRoom { get; set; }
    }
}
