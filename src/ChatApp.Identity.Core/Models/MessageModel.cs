using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Identity.Core.Models
{
    public class MessageModel : IBaseModel
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string SentByUserId { get; set; }
        public string SentToChatRoomId { get; set; }
        public string SentByName { get; set; }
        public DateTime SentAt { get; set; }
    }
}
