using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SignalRTest.Models {
    public class ChatHub : Hub {
        public async Task NewMessage(string username, string message) {
            Console.WriteLine($"{username} {message}");
            await Clients.All.SendAsync("messageReceived", username, message);
        }
    }
}