using ChatApp.Identity.Core.Models;
using ChatApp.Identity.Managers;
using ChatApp.WebApi.Chat.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ChatApp.WebApi.Chat
{
    [Route("api/rooms")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class ChatRoomController : ControllerBase
    {
        public ChatRoomManager RoomManager { get; }
        public MessageManager MessageManager { get; }
        public IHubContext<RoomHub> Hub { get; }
        public IHubContext<MessageHub> MessageHub { get; }
        public IdentityManager IdentityManager { get; }

        public CancellationToken CancellationToken => HttpContext.RequestAborted;

        public ChatRoomController(
            ChatRoomManager chatRoomManager,
            MessageManager messageManager,
            IHubContext<RoomHub> hub,
            IHubContext<MessageHub> messageHub,
            IdentityManager identityManager
        )
        {
            RoomManager = chatRoomManager;
            MessageManager = messageManager;
            Hub = hub;
            MessageHub = messageHub;
            IdentityManager = identityManager;
        }

        [HttpPost]
        [ProducesDefaultResponseType(typeof(ChatRoomModel))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ChatRoomModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ModelStateDictionary))]
        public async Task<IActionResult> CreateAsync([FromBody] ChatRoomModel model)
        {
            var result = await RoomManager.SaveAsync(model, User);
            if (result > 0)
            {
                var item = RoomManager.FindById(model.Id);
                await Hub.Clients.All.SendAsync("newRoom", item, CancellationToken);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = model.Id },
                    item
                );
            }
            
            return BadRequest(ModelState);
        }

        [HttpGet("{id}")]
        [ProducesDefaultResponseType(typeof(ChatRoomModel))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ChatRoomModel))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetById(string id)
        {
            var item = RoomManager.FindById(id);
            return Ok(item);
        }

        [HttpPost("{roomId}/messages")]
        [ProducesDefaultResponseType(typeof(MessageModel))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(MessageModel))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> SendMessageToRoom(string roomId, [FromBody] MessageModel messageModel)
        {
            await MessageManager.SaveAsync(roomId, messageModel, User);
            
            var message = MessageManager.FindById(messageModel.Id);
            await MessageHub.Clients.All.SendAsync("newMessage", message, CancellationToken);

            return CreatedAtAction(
                nameof(MessageController.GetById),
                "Message",
                new { id = message.Id },
                message
            );
        }

        [HttpGet("{roomId}/messages")]
        [ProducesDefaultResponseType(typeof(IEnumerable<MessageModel>))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MessageModel>))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetMessagesByRoomId(string roomId)
        {
            var messages = MessageManager.GetAllByRoomId(roomId);
            return Ok(messages);
        }

        [HttpGet("user-rooms/{userId}")]
        [ProducesDefaultResponseType(typeof(IEnumerable<ChatRoomModel>))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ChatRoomModel>))]
        public IActionResult GetChatRoomsByUserId(string userId)
        {
            var rooms = RoomManager.GetChatRoomsByUserId(userId);
            return Ok(rooms);
        }

        [HttpGet("user-rooms")]
        [ProducesDefaultResponseType(typeof(IEnumerable<ChatRoomModel>))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ChatRoomModel>))]
        public async Task<IActionResult> GetChatRoomsForCurrentUser()
        {
            var user = await IdentityManager.GetUserAsync(User);
            var rooms = RoomManager.GetChatRoomsByUserId(user.Id);
            return Ok(rooms);
        }


        [HttpGet]
        [ProducesDefaultResponseType(typeof(IEnumerable<ChatRoomModel>))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ChatRoomModel>))]
        public IActionResult GetAll()
        {
            var items = RoomManager.GetAll();
            return Ok(items);
        }

        [HttpGet("{roomId}/participants")]
        [ProducesDefaultResponseType(typeof(IEnumerable<ChatUserModel>))]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(IEnumerable<ChatUserModel>))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetParticipants(string roomId)
        {
            var participants = RoomManager.GetRoomParticipants(roomId);

            return Ok(participants);
        }

        [HttpGet("{roomId}/non-participants")]
        [ProducesDefaultResponseType(typeof(IEnumerable<ChatUserModel>))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ChatUserModel>))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetNonParticipants(string roomId)
        {
            var participants = RoomManager.GetNonRoomParticipants(roomId);

            return Ok(participants);
        }

        [HttpPatch("{roomId}")]
        [ProducesDefaultResponseType(typeof(ChatRoomModel))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ChatRoomModel))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateRoom(string roomId, [FromBody] string name)
        {
            await RoomManager.UpdateAsync(roomId, new ChatRoomModel() { Name = name });
            var room = RoomManager.FindById(roomId);
            return Ok(room);
        }

        [HttpPost("{roomId}/add-users-by-id")]
        [ProducesDefaultResponseType()]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> AddUsersById(string roomId, [FromBody] IEnumerable<string> userIds)
        {
            await RoomManager.AddUserToRoomAsync(userIds, roomId);
            return NoContent();
        }
    }
}
