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
    public class InputWindow
    {
        private IPAddress serverIP;
        private int serverPort;
        private TcpListener listener = new TcpListener(IPAddress.Any, 90);
        private NetworkStream outputWindowStream;
        private NetworkStream serverStream;
        private string username;
        private string sessionKey;

        public InputWindow(IPAddress serverIP, int serverPort)
        {
            this.serverIP = serverIP;
            this.serverPort = serverPort;
        }

        public void Start()
        {
            Console.Title = "Enter a message";

            Console.Clear();

            Console.ForegroundColor = ConsoleColor.White;

            Console.WindowHeight = 3;

            ConnectToOutputWindow();

            ConnectToServer();

            while (true)
            {
                EnterMessage();
            }
        }

        public void ConnectToOutputWindow()
        {
            listener.Start();

            TcpClient client = listener.AcceptTcpClient();

            GetSessionData(client);
        }

        public void ConnectToServer()
        {
            IPEndPoint server = new IPEndPoint(this.serverIP, this.serverPort);

            TcpClient client = new TcpClient();

            NetworkManager.Connect(server, client);

            this.serverStream = client.GetStream();
        }

        public void GetSessionData(TcpClient client)
        {
            this.outputWindowStream = client.GetStream();

            if (outputWindowStream.CanWrite && outputWindowStream.CanRead)
            {
                string sessionDataProtocol = NetworkManager.ReadMessage(outputWindowStream, 48);

                char[] sessionDataArray = sessionDataProtocol.ToCharArray();

                if (sessionDataArray[0] == 'C' && sessionDataArray[1] == 'H' && sessionDataArray[2] == 'A' && sessionDataArray[3] == 'T' && sessionDataArray[4] == 'S' && sessionDataArray[5] == 'D')
                {
                    string sessionDataString = string.Empty;

                    for (int i = 6; i < sessionDataArray.Length; i++)
                    {
                        sessionDataString = sessionDataString + sessionDataArray[i];
                    }

                    string[] sessionData = sessionDataString.Split('-');

                    if (sessionData.Length == 2)
                    {
                        this.username = sessionData[0];
                        this.sessionKey = sessionData[1];

                        Console.WriteLine("Session data was successfully received!");
                        Console.WriteLine("Username: {0}", this.username);
                        Console.WriteLine("Session Key: {0}", this.sessionKey);
                        Console.ReadKey();
                    }
                }
            }
        }

        public void EnterMessage()
        {
            Console.Clear();

            Console.Write(">>");

            string message = Console.ReadLine();
        }
    }
}
