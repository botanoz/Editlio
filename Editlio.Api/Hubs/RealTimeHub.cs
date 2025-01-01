using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace Editlio.Api.Hubs
{
    public class RealTimeHub : Hub
    {
        // Client bir gruba katılır
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("UserJoined", Context.ConnectionId);
        }

        // Client bir gruptan ayrılır
        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("UserLeft", Context.ConnectionId);
        }

        // Belirli bir gruba mesaj gönderir
        public async Task SendMessageToGroup(string groupName, string message)
        {
            await Clients.Group(groupName).SendAsync("ReceiveMessage", Context.ConnectionId, message);
        }

        // Tüm client'lara mesaj gönderir
        public async Task SendMessageToAll(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", Context.ConnectionId, message);
        }
    }
}
