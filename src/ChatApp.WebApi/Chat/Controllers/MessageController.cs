using ChatApp.Identity.Core.Models;
using ChatApp.Identity.Managers;
using ChatApp.WebApi.Chat.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChatApp.WebApi.Chat
{
    [Route("api/messages")]
    [ApiController]
    [Authorize]
    public class MessageController : ControllerBase
    {
        public MessageManager MessageManager { get; }
        public IHubContext<MessageHub> Hub { get; }
        public IdentityManager IdentityManager { get; }

        public CancellationToken CancellationToken => HttpContext.RequestAborted;

        public MessageController(
            MessageManager messageManager,
            IHubContext<MessageHub> hub,
            IdentityManager identityManager)
        {
            MessageManager = messageManager;
            Hub = hub;
            IdentityManager = identityManager;
        }

        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] MessageModel model)
        {
            var email = this.User.Claims.FirstOrDefault(x => x.Properties.Any(y => y.Value.Equals("sub")))?.Value;
            var user = await IdentityManager.GetUserByEmailAsync(email);

            model.SentByUserId = user.Id;
            await MessageManager.SaveAsync(model);
            var item = MessageManager.FindById(model.Id);

            await Hub.Clients.All.SendAsync("newMessage", item, CancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = model.Id },
                item
            );
        }

        [Route("{id}")]
        [HttpGet]
        public IActionResult GetById(string id)
        {
            var item = MessageManager.FindById(id);
            return Ok(item);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var items = MessageManager.GetAll();
            return Ok(items);
        }
    }
}
