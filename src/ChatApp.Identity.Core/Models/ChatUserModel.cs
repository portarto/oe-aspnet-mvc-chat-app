using System;

namespace ChatApp.Identity.Core.Models
{
    public class ChatUserModel : IBaseModel
    {
        public string Id { get; set; }
        public virtual string Username { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Email { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string Password { get; set; }
        public virtual string Details { get; set; }
        public virtual DateTime DateOfBirth { get; set; }
    }
}
