using ChatApp.Identity.Core.Models;
using ChatApp.Identity.EFCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Identity.EFCore.Mapper
{
    internal class MessageMapper : IMapper<MessageModel, Message>
    {
        public Message MapToEntity(MessageModel model)
            => new Message
            {
                Id = model.Id,
                Text = model.Text,
                SentByUserId = model.SentByUserId,
                SentToChatRoomId = model.SentToChatRoomId,
                SentAt = model.SentAt
            }
        ;

        public MessageModel MapToModel(Message entity)
            => new MessageModel
            {
                Id = entity.Id,
                Text = entity.Text,
                SentByUserId = entity.SentByUserId,
                SentToChatRoomId = entity.SentToChatRoomId,
                SentByName = entity.User.UserName,
                SentAt = entity.SentAt
            }
        ;

        public void MapForUpdate(MessageModel updatedModel, Message original)
        {
            throw new InvalidOperationException("Messages cannot be updated.");
        }
    }
}
