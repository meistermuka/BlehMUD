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
    private readonly TickSystem _tickSystem;
    private CommandParser _parser;
    private RoomManager _roomManager;
    private EventSystem _eventSystem;

    private readonly List<Player> _players = new List<Player>();
    private readonly List<NPC> _npcs = new List<NPC>();
    private readonly List<Room> _rooms = new List<Room>();
    private Dictionary<string, List<ClientHandler>> _clientsByHost = new Dictionary<string, List<ClientHandler>>();

    public MudServer(IPAddress address, int port, CommandParser parser)
    {
        _listener = new TcpListener(address, port);
        _roomManager = new RoomManager();
        _eventSystem = new EventSystem();
        _parser = parser;
        SetupRooms();
        SetupNPCs();
        SetupEventHandlers();
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
            Player newPlayer = new(client, _roomManager.GetRoomByName("Room 1"), 100, "Kronos", _eventSystem);

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

    private void SetupRooms()
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

    private void SetupNPCs()
    {
        Console.WriteLine("Setting up NPCs!");
        NPC npc1 = new("MOB 1", "A big big mob");
        NPC npc2 = new("MOB 2", "A small small mob");

        npc1.CurrentRoom = _roomManager.GetRoomByName("Room 1");
        npc2.CurrentRoom = _roomManager.GetRoomByName("Room 1");
    }

    private void SetupEventHandlers()
    {
        _eventSystem.PlayerEnteredRoom += (player, room) =>
        {
            Console.WriteLine($"{player.Name} entered {room.Name}");
            // Handle room updates or other actions
        };

        _eventSystem.PlayerExitedRoom += (player, room) =>
        {
            Console.WriteLine($"{player.Name} exited {room.Name}.");
            // Handle room updates or other actions
        };
    }
}