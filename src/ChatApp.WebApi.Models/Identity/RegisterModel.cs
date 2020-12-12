using ChatApp.Identity.Core.Models;
using ChatApp.WebApi.Models.Validation.Date;
using System;
using System.ComponentModel.DataAnnotations;

namespace ChatApp.WebApi.Models.Identity
{
    public class RegisterModel : ChatUserModel
    {
        [Required]
        public override string Username { get; set; }

        [Required]
        public override string FirstName { get; set; }

        [Required]
        public override string LastName { get; set; }

        [Required]
        [EmailAddress]
        public override string Email { get; set; }

        [Phone]
        public override string PhoneNumber { get; set; }

        [Required]
        public override string Password { get; set; }

        public override string Details { get; set; }

        [Required]
        [DateOfBirthValidation]
        public override DateTime DateOfBirth { get; set; }
    }
}
