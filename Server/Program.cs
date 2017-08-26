using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.IO;

namespace Server
{
    public class Program
    {
        private List<User> onlineUser = new List<User>();
        private static bool serverRunning = false;
        private static TcpListener listener;
        private static UserRequestManager userRequestManager = new UserRequestManager();
        private static Settings settings = new Settings();

        public static void Main(string[] args)
        {
            ShowHeader();

            Console.ForegroundColor = ConsoleColor.White;
            Console.CursorVisible = false;

            Console.WriteLine();
            Console.WriteLine("[S] Start server [Q] Stop server [O] Show online users [E] Exit server application");
            Console.WriteLine();

            while (true)
            {
                ConsoleKeyInfo cki = Console.ReadKey(true);

                // Start the server
                if (cki.Key == ConsoleKey.S)
                {
                    StartServer();
                }
                // Stop the server
                else if (cki.Key == ConsoleKey.Q)
                {
                    StopServer();
                }
                // Exit the server application
                else if (cki.Key == ConsoleKey.E)
                {
                    StopServer();

                    break;
                }
                // Displays the users who are online
                else if (cki.Key == ConsoleKey.O)
                {

                }
            }
        }

        public static void ShowHeader()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.WriteLine(" ______     __  __     ______     ______");
            Console.WriteLine("/\\  ___\\   /\\ \\_\\ \\   /\\  __ \\   /\\__  _\\");
            Console.WriteLine("\\ \\ \\____  \\ \\  __ \\  \\ \\  __ \\  \\/_/\\ \\/");
            Console.WriteLine(" \\ \\_____\\  \\ \\_\\ \\_\\  \\ \\_\\ \\_\\    \\ \\_\\");
            Console.WriteLine("  \\/_____/   \\/_/\\/_/   \\/_/\\/_/     \\/_/");

            Console.ResetColor();
        }

        public static void StartServer()
        {
            if (serverRunning == true)
            {
                Console.WriteLine("The server is already running! You can't start the server again!");
            }
            else if (serverRunning == false)
            {
                settings.CheckConfigFile();

                listener = new TcpListener(IPAddress.Any, settings.ServerPort);

                Thread startServer = new Thread(StartListening);

                serverRunning = true;

                Console.WriteLine("The sever has been started successfully!");

                startServer.Start();
            }
        }

        public static void StopServer()
        {
            if (serverRunning == true)
            {
                serverRunning = false;

                listener.Stop();

                Console.WriteLine("The server has been stopped successfully!");
            }
            else if (serverRunning == false)
            {
                Console.WriteLine("The server isn't running! You can't stop the server!");
            }
        }

        public static void StartListening()
        {
            listener.Start();

            while (serverRunning == true)
            {
                if (!listener.Pending())
                {
                    Thread.Sleep(100);

                    continue;
                }

                TcpClient client = listener.AcceptTcpClient();
                Thread session = new Thread(new ParameterizedThreadStart(HandleNewSession));
                session.Start(client);
            }
        }

        public static void HandleNewSession(object data)
        {
            TcpClient client = (TcpClient)data;

            NetworkStream stream = client.GetStream();

            if (stream.CanWrite && stream.CanRead)
            {
                string userRequest = NetworkManager.ReadMessage(client, 28);

                userRequestManager.HandleUserRequest(userRequest, client, settings.ServerUserPath);
            }
        }
    }
}
