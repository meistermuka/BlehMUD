// See https://aka.ms/new-console-template for more information
using BlehMUD;
using System.Net;

class Program
{
    static async Task Main(string[] args)
    {   
        CommandParser parser = new CommandParser();
        MudServer server = new MudServer(IPAddress.Any, 8888, parser);
        await server.StartAsync();
    }
}