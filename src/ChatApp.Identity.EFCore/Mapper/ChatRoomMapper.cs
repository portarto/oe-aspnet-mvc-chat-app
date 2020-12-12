using ChatApp.Identity.Core.Models;
using ChatApp.Identity.EFCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Identity.EFCore.Mapper
{
    internal class ChatRoomMapper : IMapper<ChatRoomModel, ChatRoom>
    {
        public ChatRoom MapToEntity(ChatRoomModel model)
            => new ChatRoom
            {
                Id = model.Id,
                Name = model.Name
            }
        ;

        public ChatRoomModel MapToModel(ChatRoom entity)
            => new ChatRoomModel
            {
                Id = entity.Id,
                Name = entity.Name
            }
        ;

        public void MapForUpdate(ChatRoomModel updatedModel, ChatRoom original)
        {
            original.Name = updatedModel.Name;
        }
    }
}
