using BlehMUD.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BlehMUD.Core
{
    public class ClientHandler
    {
        private readonly TcpClient _client;
        private NetworkStream _stream;
        private Player _player;
        private readonly CommandParser _parser;

        public ClientHandler(TcpClient client, CommandParser parser)
        {
            _client = client;
            _parser = parser;
        }

        public async Task HandleClientAsync()
        {
            try
            {
                _stream = _client.GetStream();
                _player = new Player();
                byte[] buffer = new byte[1024];
                string receivedData = string.Empty;

                await SendWelcomeMsg();
                while(true)
                {
                    int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
                    string data = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    receivedData += data;

                    if (receivedData.EndsWith("\r\n"))
                    {
                        string command = receivedData.Trim();

                        if (string.IsNullOrWhiteSpace(command))
                        {
                            await SendPrompt();
                            continue;
                        }

                        if (command.ToLower() == "quit")
                        {
                            await SendToClientAsync("Goodbye!\r\n");
                            Console.WriteLine($"Client disconnected: {_client.Client.RemoteEndPoint}");
                            _client.Close();
                            break;
                        }
                        string response = _parser.ParseAndExecute(command);//ProcessInput(input);
                        await SendToClientAsync(response);
                        await SendPrompt();
                        receivedData = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex}");
            }
            finally 
            { 
                _stream.Close();
                _client.Close();
            }
        }

        public async Task SendWelcomeMsg()
        {
            string welcomeMsg = "Welcome to BlehMUD!\r\n";
            await SendToClientAsync(welcomeMsg);
        }

        public async Task SendPrompt()
        {
            string promptString = "> ";
            await SendToClientAsync(promptString);
        }

        private async Task<string> ReadFromClientAsync()
        {
            byte[] buffer = new byte[1024];
            int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
            return Encoding.ASCII.GetString(buffer, 0, bytesRead);
        }

        private async Task SendToClientAsync(string msg)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(msg);
            await _stream.WriteAsync(messageBytes, 0, messageBytes.Length);
        }

        private string ProcessInput(string input)
        {
            return $"You typed: {input}";
        }
    }
}
