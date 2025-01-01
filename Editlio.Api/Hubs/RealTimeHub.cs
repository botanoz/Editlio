using Microsoft.AspNetCore.SignalR;

namespace Editlio.Api.Hubs
{
    public class RealTimeHub : Hub
    {
        // Client bir gruba katılır
        public async Task JoinGroup(string groupName)
        {
            if (string.IsNullOrWhiteSpace(groupName))
            {
                throw new ArgumentException("Group name cannot be empty.", nameof(groupName));
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("UserJoined", Context.ConnectionId);
        }

        // Client bir gruptan ayrılır
        public async Task LeaveGroup(string groupName)
        {
            if (string.IsNullOrWhiteSpace(groupName))
            {
                throw new ArgumentException("Group name cannot be empty.", nameof(groupName));
            }

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("UserLeft", Context.ConnectionId);
        }

        // Belirli bir gruba mesaj gönderir
        public async Task SendMessageToGroup(string groupName, string message)
        {
            if (string.IsNullOrWhiteSpace(groupName))
            {
                throw new ArgumentException("Group name cannot be empty.", nameof(groupName));
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("Message cannot be empty.", nameof(message));
            }

            await Clients.Group(groupName).SendAsync("ReceiveMessage", Context.ConnectionId, message);
        }

        // Tüm client'lara mesaj gönderir
        public async Task SendMessageToAll(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("Message cannot be empty.", nameof(message));
            }

            await Clients.All.SendAsync("ReceiveMessage", Context.ConnectionId, message);
        }

        // Tüm client'lara metin güncellemesi gönderir
        public async Task UpdateText(string newText)
        {
            if (string.IsNullOrWhiteSpace(newText))
            {
                throw new ArgumentException("Text cannot be empty.", nameof(newText));
            }

            await Clients.Others.SendAsync("UpdateText", newText); // Gönderen haricindeki tüm client'lara iletir
        }
    }
}
