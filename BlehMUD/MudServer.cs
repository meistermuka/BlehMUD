using BlehMUD.Constants;
using BlehMUD.Core;
using BlehMUD.Entities;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Text;

namespace BlehMUD;

public class MudServer
{
    private TcpListener _listener;
    private readonly CommandParser _parser;
    private readonly TickSystem _tickSystem;

    private readonly List<Player> _players = new List<Player>();
    private readonly List<NPC> _npcs = new List<NPC>();
    private readonly List<Room> _rooms = new List<Room>();
    private Dictionary<string, List<ClientHandler>> _clientsByHost = new Dictionary<string, List<ClientHandler>>();

    public MudServer(IPAddress address, int port, CommandParser parser)
    {
        _listener = new TcpListener(address, port);
        _parser = parser;
        _tickSystem = new TickSystem(tickIntervalMilliseconds: CoreConstants.TICKINTERVALMS);
        _tickSystem.Tick += OnTick;
    }

    private void OnTick(object sender, EventArgs e)
    {
        foreach(Player p in _players)
        {
            Console.WriteLine("Looping through players!");
            string clientHost = ((IPEndPoint)p.Client.Client.RemoteEndPoint).Address.ToString();
            SendMessageToHost(clientHost, "Tick!\r\n");
        }
        Console.WriteLine("Server Tick!");
    }

    public async Task StartAsync()
    {
        _listener.Start();
        Console.WriteLine("BlehMUD started...");

        while(true)
        {
            TcpClient client = await _listener.AcceptTcpClientAsync();
            Console.WriteLine($"Client connected: {client.Client.RemoteEndPoint}");
            ClientHandler clientHandler = new(client, _parser);
            string clientHost = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
            if(!_clientsByHost.ContainsKey(clientHost))
            {
                _clientsByHost.Add(clientHost, new List<ClientHandler>());
            }
            _clientsByHost[clientHost].Add(clientHandler);
            _players.Add(new Player
            {
                Client = client
            });
            await clientHandler.HandleClientAsync();
        }
    }

    public async Task SendMessageToHost(string host, string message)
    {
        if(_clientsByHost.ContainsKey(host))
        {
            foreach (var clientHandler in _clientsByHost[host])
            {
                await clientHandler.SendToClientAsync(message);
            }
        }
    }

    private Player GetPlayerByClient(TcpClient client)
    {
        return _players.FirstOrDefault(player => player.Client == client);
    }

    private void SendResponseToClient(Player player, string response)
    {
        try
        {
            byte[] responseBytes = Encoding.ASCII.GetBytes(response);
            player.Client.GetStream().Write(responseBytes, 0, responseBytes.Length);   
        } 
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        
    }

    private void HandleClientAsync(TcpClient client)
    {
        try
        {
            using (NetworkStream stream = client.GetStream())
            {
                byte[] buffer = new byte[1024];
                int bytesRead;
                string receivedData = string.Empty;

                while (true)
                {
                    Console.WriteLine($"====== buffer: {buffer}");
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string data = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    receivedData += data;

                    // Check if the received data ends with CRLF
                    if (receivedData.EndsWith("\r\n"))
                    {
                        string command = receivedData.Trim();
                        if (command.ToLower() == "quit")
                        {
                            Console.WriteLine($"Client disconnected: {client.Client.RemoteEndPoint}");
                            client.Close();
                            return;
                        }
                        string response = _parser.ParseAndExecute(command);
                        Player player = GetPlayerByClient(client);
                        if (player != null)
                        {
                            SendResponseToClient(player, response);
                        }
                        // Clear received data for the next command
                        receivedData = string.Empty;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex}");
        }
    }
}