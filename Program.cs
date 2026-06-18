using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    static void Main()
    {
        var server = new TcpListener(IPAddress.Loopback, 5000);
        server.Start();

        Console.WriteLine("Server started on 127.0.0.1:5000");

        using var client = server.AcceptTcpClient();
        Console.WriteLine("Client connected");

        using var stream = client.GetStream();
        byte[] buffer = new byte[1024];

        while (true)
        {
            int n = stream.Read(buffer, 0, buffer.Length);
            if (n == 0) break;

            string msg = Encoding.UTF8.GetString(buffer, 0, n).Trim();

            Console.WriteLine("Client: " + msg);

            if (msg.ToLower() == "exit")
            {
                Console.WriteLine("Exit command received");
                break;
            }

            stream.Write(buffer, 0, n);
            Console.WriteLine("Echo sent: " + msg);
        }

        Console.WriteLine("Server stopped");
        client.Close();
        server.Stop();
    }
}