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
    private RoomManager _roomManager;

    private readonly List<Player> _players = new List<Player>();
    private readonly List<NPC> _npcs = new List<NPC>();
    private readonly List<Room> _rooms = new List<Room>();
    private Dictionary<string, List<ClientHandler>> _clientsByHost = new Dictionary<string, List<ClientHandler>>();

    public MudServer(IPAddress address, int port, CommandParser parser)
    {
        _listener = new TcpListener(address, port);
        _parser = parser;
        _roomManager = new RoomManager();
        SetupRooms();
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
            Player newPlayer = new() {
                Client = client,
                CurrentRoom = _roomManager.GetRoomByName("Room 1")
            };
            Console.WriteLine($"Client connected: {client.Client.RemoteEndPoint}");

            ClientHandler clientHandler = new(client, _parser, newPlayer);

            string clientHost = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();

            if(!_clientsByHost.ContainsKey(clientHost))
            {
                _clientsByHost.Add(clientHost, new List<ClientHandler>());
            }
            _clientsByHost[clientHost].Add(clientHandler);
            _players.Add(newPlayer);
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

    public void SetupRooms()
    {
        Console.WriteLine("Setting up rooms!");
        Room room1 = new("Room 1", "Room 1 description");
        Room room2 = new("Room 2", "Room 2 description");
        Room room3 = new("Room 3", "Room 3 description");

        room1.Exits["north"] = room2;
        room2.Exits["south"] = room1;
        room2.Exits["up"] = room3;
        room3.Exits["down"] = room2;

        _roomManager.AddRoom(room1);
        _roomManager.AddRoom(room2);
        _roomManager.AddRoom(room3);
    }
}