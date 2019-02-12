using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace Client {
    internal class Program {
        private static HubConnection _connection;
        private static List<string> _messages;

        private static void Main(string[] args) {
            Console.WriteLine("Hello World!");
            _messages = new List<string>();

            _connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/Hub")
                .Build();

            _connection.Closed += async error => {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await _connection.StartAsync();
            };

            Connect();

            do {
                var input = Console.ReadLine();
                if (string.IsNullOrEmpty(input) == false) input = input.ToLower();

                if (input == "quit" || input == "exit") break;
                Send(input);
            } while (true);
        }

        private static async void Connect() {
            _connection.On<string, string>("messageReceived", (user, message) => {
                var newMessage = $"{user}: {message}";
                Console.WriteLine($"收到消息:{newMessage}");
                _messages.Add(newMessage);
            });

            try {
                await _connection.StartAsync();
                _messages.Add("Connection started");
            }
            catch (Exception ex) {
                _messages.Add(ex.Message);
            }
        }

        private static async void Send(string input) {
            try {
                await _connection.InvokeAsync("NewMessage",
                    "mhw", input);
            }
            catch (Exception ex) {
                _messages.Add(ex.Message);
            }
        }
    }
}