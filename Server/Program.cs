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
                    listener.Stop();

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
            serverRunning = false;

            listener.Stop();

            Console.WriteLine("The server has been stopped successfully!");
        }

        public static void StartListening()
        {
            listener.Start();

            while (serverRunning == true)
            {
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
                string userRequest = ReadMessage(client, 28);

                HandleUserRequest(userRequest, client);
            }
        }

        public static void HandleUserRequest(string userRequest, TcpClient client)
        {
            char[] splittedUserRequest = userRequest.ToCharArray();

            if (splittedUserRequest[0] == 'C' && splittedUserRequest[1] == 'H' && splittedUserRequest[2] == 'A' && splittedUserRequest[3] == 'T')
            {
                string username = string.Empty;
                string password = string.Empty;

                for (int i = 6; i < splittedUserRequest.Length; i++)
                {
                    if (splittedUserRequest[i] == '-')
                    {
                        for (int j = i + 1; j < splittedUserRequest.Length; j++)
                        {
                            password = password + splittedUserRequest[j];
                        }

                        break;
                    }
                    else
                    {
                        username = username + splittedUserRequest[i];
                    }
                }

                if (splittedUserRequest[4] == 'R' && splittedUserRequest[5] == 'R')
                {
                    CreateNewUser(username, password, client);
                }
                else if (splittedUserRequest[4] == 'L' && splittedUserRequest[5] == 'R')
                {
                    // Login Request
                }
            }
        }

        public static void CreateNewUser(string username, string password, TcpClient client)
        {
            string path = Directory.GetCurrentDirectory();

            path = path + @"\User\" + username + ".txt";

            if (File.Exists(path) == true) 
            {
                Protocol registrationInvalid = ProtocolCreator.RegistrationInvalid();

                SendMessage(registrationInvalid, client);
            }
            else
            {
                if (CheckIfLegalUserData(username, password) == false)
                {
                    Protocol registrationInvalid = ProtocolCreator.RegistrationInvalid();

                    SendMessage(registrationInvalid, client);
                }
                else
                {
                    string[] userFile = new string[3];

                    userFile[0] = "Username: " + username;
                    userFile[1] = "Password: " + password;
                    userFile[2] = "User Group: User";

                    File.WriteAllLines(path, userFile);

                    Protocol registrationOk = ProtocolCreator.RegistrationOk();

                    SendMessage(registrationOk, client);
                }
            }
        }

        public static void SendMessage(Protocol protocol, TcpClient client)
        {
            NetworkStream stream = client.GetStream();

            byte[] sendBuffer = protocol.Create();

            stream.Write(sendBuffer, 0, sendBuffer.Length);
        }

        public static string ReadMessage(TcpClient client, int messageLength)
        {
            NetworkStream stream = client.GetStream();

            byte[] receiveBuffer = new byte[messageLength];

            int receivedBytes = stream.Read(receiveBuffer, 0, receiveBuffer.Length);

            string message = Encoding.ASCII.GetString(receiveBuffer, 0, receivedBytes);

            return message;
        }

        public static bool CheckIfLegalUserData(string username, string password)
        {
            char[] splittedUsername = username.ToCharArray();
            char[] splittedPassword = password.ToCharArray();

            // Checks the length of the username and password
            if (CheckIfLegalString(username, 3, 10) == false)
            {
                return false;
            }
            else if (CheckIfLegalString(password, 6, 12) == false)
            {
                return false;
            }

            // Checks the characters of the username and password
            for (int i = 0; i < splittedUsername.Length; i++)
            {
                if (CheckIfLegalCharacter(splittedUsername[i]) == false)
                {
                    return false;
                }
            }

            for (int i = 0; i < splittedPassword.Length; i++)
            {
                if (CheckIfLegalCharacter(splittedPassword[i]) == false)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool CheckIfLegalCharacter(char character)
        {
            int charNumber = (int)character;

            // a-z
            if (charNumber >= 97 && charNumber <= 122)
            {
                return true;
            }
            // A-Z
            else if (charNumber >= 65 && charNumber <= 90)
            {
                return true;
            }
            // 0-9
            else if (charNumber >= 48 && charNumber <= 57)
            {
                return true;
            }
            // ü, Ü, ä, Ä, ö, Ö
            else if (charNumber == 252 || charNumber == 220 || charNumber == 228 || charNumber == 196 || charNumber == 246 || charNumber == 214)
            {
                return true;
            }
            // #, !, ?, $, %, &, ~, @
            else if (charNumber == 35 || charNumber == 33 || charNumber == 63 || charNumber == 36 || charNumber == 37 || charNumber == 38 || charNumber == 126 || charNumber == 64)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool CheckIfLegalString(string text, int minLength, int maxLength)
        {
            if (string.IsNullOrEmpty(text) == true)
            {
                return false;
            }
            else if (text.Length < minLength || text.Length > maxLength)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
