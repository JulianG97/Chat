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
        private static TcpListener listener = new TcpListener(IPAddress.Any, 80);
        private static UserRequestManager userRequestManager = new UserRequestManager();

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
                Console.WriteLine("The server has been stopped already! You can't stop the server again!");
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

                userRequestManager.HandleUserRequest(userRequest, client);
            }
        }

        public static void CheckConfigFile(out int port, out string userPath)
        {
            string path = Directory.GetCurrentDirectory();
            path = path + @"\config.txt";

            if (File.Exists(path) == true)
            {
                string[] configContent = File.ReadAllLines(path);

                port = CheckConfigPort(configContent[0]);
            }
            else if (File.Exists(path) == false)
            {

            }
        }

        public static void GenerateDefaultConfigFile()
        {

        }

        public static int CheckConfigPort(string portLine)
        {
            char[] portArray = portLine.ToCharArray();

            int port;

            if (portArray[0] == 'P' && portArray[1] == 'O' && portArray[2] == 'R' && portArray[3] == 'T' && portArray[4] == ':' && portArray[5] == ' ')
            {
                string portString = string.Empty;

                for (int i = 6; i < portArray.Length; i++)
                {
                    portString = portString + portArray[i];
                }

                if (int.TryParse(portString, out port) == true)
                {
                    return port;
                }
                else if (int.TryParse(portString, out port) == false)
                {
                    GenerateDefaultConfigFile();
                }
            }
            else
            {
                GenerateDefaultConfigFile();
            }
        }

        public static void CheckConfigUserPath()
        {

        }
    }
}
