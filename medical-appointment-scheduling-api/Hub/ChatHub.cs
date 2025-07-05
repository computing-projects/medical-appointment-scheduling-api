using medical_appointment_scheduling_api.Models;
using Microsoft.AspNetCore.SignalR;

namespace medical_appointment_scheduling_api.Hub
{
    public class ChatHub : Hub
    {
        public async Task SendMessageToClient(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
