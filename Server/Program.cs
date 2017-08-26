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
        private static int serverPort;
        private static string serverUserPath;

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
                CheckConfigFile();

                listener = new TcpListener(IPAddress.Any, serverPort);

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

        public static void CheckConfigFile()
        {
            int port;
            string userPath;

            string configPath = Directory.GetCurrentDirectory();
            configPath = configPath + @"\config.txt";

            if (File.Exists(configPath) == true)
            {
                string[] configContent = File.ReadAllLines(configPath);

                CheckConfigPort(configContent[0], out port);

                if (port == 0)
                {
                    GenerateDefaultConfigFile();
                }
                else if (port != 0)
                {
                    CheckConfigUserPath(configContent[1], out userPath);

                    if (userPath == string.Empty)
                    {
                        GenerateDefaultConfigFile();
                    }
                    else if (userPath != string.Empty)
                    {
                        serverPort = port;
                        serverUserPath = userPath;
                    }
                }
            }
            else if (File.Exists(configPath) == false)
            {
                GenerateDefaultConfigFile();
            }
        }

        public static void GenerateDefaultConfigFile()
        {
            string configPath = Directory.GetCurrentDirectory();

            string[] configFile = new string[2];

            configFile[0] = "Port: 80";
            configFile[1] = "User Path: " + configPath + @"\User";

            File.WriteAllLines(configPath + @"\config.txt", configFile);
        }

        public static void CheckConfigPort(string portLine, out int port)
        {
            char[] portArray = portLine.ToCharArray();
            port = 0;

            if (portArray[0] == 'P' && portArray[1] == 'o' && portArray[2] == 'r' && portArray[3] == 't' && portArray[4] == ':' && portArray[5] == ' ')
            {
                string portString = string.Empty;

                for (int i = 6; i < portArray.Length; i++)
                {
                    portString = portString + portArray[i];
                }

                int.TryParse(portString, out port);
            }
        }

        public static void CheckConfigUserPath(string userPathLine, out string userPath)
        {
            char[] userPathArray = userPathLine.ToCharArray();
            userPath = string.Empty;

            if (userPathArray[0] == 'U' && userPathArray[1] == 's' && userPathArray[2] == 'e' && userPathArray[3] == 'r' && userPathArray[4] == ' ' && userPathArray[5] == 'P' && userPathArray[6] == 'a' && userPathArray[7] == 't' && userPathArray[8] == 'h' && userPathArray[9] == ':' && userPathArray[10] == ' ')
            {
                string userPathString = string.Empty;

                for (int i = 11; i < userPathArray.Length; i++)
                {
                    userPathString = userPathString + userPathArray[i];
                }

                if (Directory.Exists(userPathString) == true)
                {
                    userPath = userPathString;
                }
                else if (Directory.Exists(userPathString) == false)
                {
                    userPath = Directory.GetCurrentDirectory() + @"\User";

                    Directory.CreateDirectory(userPath);
                }
            }
        }
    }
}
