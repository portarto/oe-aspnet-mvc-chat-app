using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Identity.Core.Models
{
    public class ChatRoomModel : IBaseModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
