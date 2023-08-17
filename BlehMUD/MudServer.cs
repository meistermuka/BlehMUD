using System.Net;
using System.Net.Sockets;
using System.Text;

namespace BlehMUD;

public class MudServer
{
    private TcpListener _listener;
    private readonly CommandParser _parser;

    public MudServer(IPAddress address, int port, CommandParser parser)
    {
        _listener = new TcpListener(address, port);
        _parser = parser;
    }

    public async Task StartAsync()
    {
        _listener.Start();
        Console.WriteLine("BlehMUD started...");

        while(true)
        {
            TcpClient client = await _listener.AcceptTcpClientAsync();
            Console.WriteLine($"Client connected: {client.Client.RemoteEndPoint}");
            await HandleClientAsync(client);
        }
    }

    private async Task HandleClientAsync(TcpClient client)
    {
        /*using (NetworkStream stream = client.GetStream())
        {
            byte[] buffer = new byte[1024];
            int bytesRead;

            while((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                string command = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                command = command.Trim();

                string response = _parser.ParseAndExecute(command);

                byte[] responseBytes = Encoding.ASCII.GetBytes(response);
                await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
            }
        }*/
        using (NetworkStream stream = client.GetStream())
        {
            byte[] buffer = new byte[1024];
            int bytesRead;
            string receivedData = string.Empty;

            while (true)
            {
                bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                string data = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                receivedData += data;

                // Check if the received data ends with CRLF
                if (receivedData.EndsWith("\r\n"))
                {
                    string command = receivedData.Trim();
                    string response = _parser.ParseAndExecute(command);

                    byte[] responseBytes = Encoding.ASCII.GetBytes(response);
                    await stream.WriteAsync(responseBytes, 0, responseBytes.Length);

                    // Clear received data for the next command
                    receivedData = string.Empty;
                }
            }
        }
        //client.Close();
    }
}