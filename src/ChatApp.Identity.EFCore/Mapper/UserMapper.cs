using ChatApp.Identity.Core.Models;
using ChatApp.Identity.EFCore.Entities;

namespace ChatApp.Identity.EFCore.Mapper
{
    internal class UserMapper : IMapper<ChatUserModel, ChatUser>
    {
        public ChatUser MapRegisterModelToEntity(ChatUserModel model)
            => new ChatUser
            {
                UserName = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Details = model.Details,
                DateOfBirth = model.DateOfBirth
            }
        ;

        public ChatUser MapToEntity(ChatUserModel model)
            => new ChatUser
            {
                Id = model.Id,
                UserName = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Details = model.Details,
                DateOfBirth = model.DateOfBirth
            }
        ;

        public ChatUserModel MapToModel(ChatUser entity)
            => entity is null
            ? null
            : new ChatUserModel
            {
                Id = entity.Id,
                Username = entity.UserName,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
                Details = entity.Details,
                DateOfBirth = entity.DateOfBirth
            }
        ;

        public void MapForUpdate(ChatUserModel updatedModel, ChatUser original)
        {
            original.FirstName = updatedModel.FirstName;
            original.LastName = updatedModel.LastName;
            original.DateOfBirth = updatedModel.DateOfBirth;
            original.PhoneNumber = updatedModel.PhoneNumber;
            original.Details = updatedModel.Details;
        }
    }
}
