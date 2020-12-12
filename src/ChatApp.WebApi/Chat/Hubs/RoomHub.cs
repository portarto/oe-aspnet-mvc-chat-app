using ChatApp.Identity.Core.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChatApp.WebApi.Chat.Hubs
{
    public class RoomHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            Clients.Caller.SendAsync("Connected", Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public Task SendMessageAsync(MessageModel messageModel, CancellationToken cancellationToken)
            => Clients.All.SendAsync("newRoom", messageModel, cancellationToken);
    }
}
