using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Client
{
    public class OutputWindow
    {
        private string username;
        private string sessionKey;
        private NetworkStream serverStream;
        private NetworkStream inputWindowStream;
        private Thread forwardMessages;

        public OutputWindow(string username, string sessionKey, NetworkStream serverStream)
        {
            this.username = username;
            this.sessionKey = sessionKey;
            this.serverStream = serverStream;
        }

        public void Start()
        {
            Console.Clear();

            Menu.ShowHeader();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.White;

            ConnectToInputWindow();

            DisplayMessagesReceivedFromServer();
        }

        public void ConnectToInputWindow()
        {
            IPEndPoint outputWindow = new IPEndPoint(IPAddress.Loopback, 90);

            TcpClient client = new TcpClient();

            NetworkManager.Connect(outputWindow, client);

            this.inputWindowStream = client.GetStream();

            Protocol sessionData = ProtocolCreator.SessionData(this.username, this.sessionKey);

            NetworkManager.SendMessage(sessionData, this.inputWindowStream);

            this.forwardMessages = new Thread(ForwardMessagesToServer);
            forwardMessages.Start();
        }

        public void DisplayMessagesReceivedFromServer()
        {
            while (true)
            {
                string messageProtocol = NetworkManager.ReadMessage(serverStream, 276);

                char[] mesageProtocolArray = messageProtocol.ToCharArray();

                if (messageProtocol[0] == 'C' && messageProtocol[1] == 'H' && messageProtocol[2] == 'A' && messageProtocol[3] == 'T' && messageProtocol[4] == 'P' && messageProtocol[5] == 'M')
                {
                    string messageString = string.Empty;

                    for (int i = 6; i < mesageProtocolArray.Length; i++)
                    {
                        messageString = messageString + mesageProtocolArray[i];
                    }

                    string[] messageProtocolContent = messageString.Split('-');

                    WriteMessage(messageProtocolContent[0], messageProtocolContent[1], messageProtocolContent[2], messageProtocolContent[3]);
                }
            }
        }

        public void WriteMessage(string username, string userGroup, string time, string message)
        {
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.White;

            Console.Write("[" + time + "] ");

            switch (userGroup)
            {
                case "A":
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case "M":
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
            }

            Console.Write(username + ": ");

            Console.ForegroundColor = ConsoleColor.White;

            Console.Write(message);
        }

        public void ForwardMessagesToServer()
        {
            while (true)
            {
                string messageProtocol = NetworkManager.ReadMessage(inputWindowStream, 302);

                char[] mesageProtocolArray = messageProtocol.ToCharArray();

                if (messageProtocol[0] == 'C' && messageProtocol[1] == 'H' && messageProtocol[2] == 'A' && messageProtocol[3] == 'T' && messageProtocol[4] == 'M' && messageProtocol[5] == 'E')
                {
                    string messageString = string.Empty;

                    for (int i = 6; i < mesageProtocolArray.Length; i++)
                    {
                        messageString = messageString + mesageProtocolArray[i];
                    }

                    string[] messageProtocolContent = messageString.Split('-');

                    Protocol message = ProtocolCreator.Message(messageProtocolContent[0], messageProtocolContent[1], messageProtocolContent[2]);
                    NetworkManager.SendMessage(message, serverStream);
                }
            }
        }
    }
}
