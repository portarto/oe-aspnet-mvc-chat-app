using ChatApp.Core.Models;
using ChatApp.Identity.Core.Models;
using ChatApp.Identity.Managers;
using ChatApp.WebApi.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.WebApi.Controllers
{
    [Route("api/identities")]
    [ApiController]
    public class IdentityController : Controller
    {
        public IConfiguration Configuration { get; }
        public IdentityManager IdentityManager { get; }

        public IdentityController(
            IConfiguration configuration,
            IdentityManager identityManager
        )
        {
            Configuration = configuration;
            IdentityManager = identityManager;
        }

        [HttpGet]
        [ProducesDefaultResponseType(typeof(IEnumerable<ChatUserModel>))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ChatUserModel>))]
        public IActionResult GetAllUsers()
        {
            var users = IdentityManager.Users;
            return Ok(users);
        }

        [HttpGet("by-email/{email}")]
        [ProducesDefaultResponseType(typeof(ChatUserModel))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ChatUserModel))]
        public IActionResult GetByEmail(string email)
        {
            var user = IdentityManager.GetUserByEmailAsync(email);
            return Ok(user);
        }

        [HttpGet("by-usernae/{username}")]
        [ProducesDefaultResponseType(typeof(ChatUserModel))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ChatUserModel))]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            var user = await IdentityManager.GetUserByUsernameAsync(username);
            return Ok(user);
        }

        [HttpGet("{id}")]
        [ProducesDefaultResponseType(typeof(ChatUserModel))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ChatUserModel))]
        public IActionResult GetById(string id)
        {
            var user = IdentityManager.GetById(id);
            return Ok(user);
        }

        [HttpPost("register")]
        [ProducesDefaultResponseType(typeof(ChatUserModel))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ChatUserModel))]
        public async Task<ActionResult> RegisterUser([FromBody] RegisterModel model)
        {
            var email = await IdentityManager.RegisterUser(model);
            var user = await IdentityManager.GetUserByEmailAsync(email);

            return CreatedAtAction(
                nameof(GetByEmail),
                new { email = email },
                user
            );
        }

        [HttpPatch]
        [ProducesDefaultResponseType(typeof(ChatUserModel))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ChatUserModel))]
        public async Task<IActionResult> UpdateUser([FromBody] ChatUserModel userModel)
        {
            await IdentityManager.UpdateUser(userModel, User);
            var user = IdentityManager.GetUserByEmailAsync(userModel.Email);
            return Ok(user);
        }

        [HttpPost("login")]
        [ProducesDefaultResponseType(typeof(TokenModel))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Login([FromBody] LoginModel model)
        {
            var (opRes, result) = await IdentityManager.GetToken(model.Email, model.Password);
            if (opRes.IsSuccess)
            {
                return Ok(MapToken(result));
            }

            return Unauthorized();
        }

        [Authorize]
        [HttpGet("current-user")]
        [ProducesDefaultResponseType(typeof(ChatUserModel))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ChatUserModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await IdentityManager.GetUserAsync(User);
            return Ok(user);
        }

        private TokenModel MapToken(JwtSecurityToken token)
            => new TokenModel
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            }
        ;
    }
}
