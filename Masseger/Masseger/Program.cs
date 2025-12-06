using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net.Sockets;

namespace Masseger
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Select mode: 1. Server.  2. Client.");
            string mode = Console.ReadLine();

            if (mode=="1")
            {
                StartServer();
            }
            else
            {
                StartClient();
            }
        }
        static void StartServer()
        {
            TcpListener server = new TcpListener(System.Net.IPAddress.Any,5000);
            server.Start();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Server started...");

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                NetworkStream stream = client.GetStream();

                Thread thread = new Thread(() =>
                {
                    while (true)
                    {
                        byte[] buffer = new byte[1024];
                        int byteRead = stream.Read(buffer,0,buffer.Length);
                        string message = Encoding.UTF8.GetString(buffer,0,byteRead);
                        Console.WriteLine("Client: "+message);
                    }
                });
                thread.Start();
            }
        }
        static void StartClient()
        {
            TcpClient client = new TcpClient("127.0.0.1",5000);
            NetworkStream stream = client.GetStream();

            Thread thread = new Thread(() =>
                {
                    while (true)
                    {
                        byte[]buffer=new byte[1024];
                        int byteRead = stream.Read(buffer,0,buffer.Length);
                        string message = Encoding.UTF8.GetString(buffer,0,byteRead);
                        Console.WriteLine("Server: "+message);
                    }
                });
            thread.Start();

            while (true)
            {
                string message = Console.ReadLine();
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                stream.Write(buffer,0,buffer.Length);
            }
        }
    }
}
