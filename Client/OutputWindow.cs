using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    public class OutputWindow
    {
        private string username;
        private string sessionKey;

        public OutputWindow(string username, string sessionKey)
        {
            this.username = username;
            this.sessionKey = sessionKey;
        }

        public void Start()
        {
            Console.Clear();

            Menu.ShowHeader();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.White;

            ConnectToInputWindow();
        }

        public void ConnectToInputWindow()
        {
            IPEndPoint outputWindow = new IPEndPoint(IPAddress.Loopback, 90);

            TcpClient client = new TcpClient();

            NetworkManager.Connect(outputWindow, client);

            NetworkStream stream = client.GetStream();

            Protocol sessionData = ProtocolCreator.SessionData(this.username, this.sessionKey);

            NetworkManager.SendMessage(sessionData, stream);
        }
    }
}
