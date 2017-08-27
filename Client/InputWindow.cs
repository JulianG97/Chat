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
        private TcpListener listener = new TcpListener(IPAddress.Any, 90);

        public void Start()
        {
            Console.Title = "Enter a message";

            Console.Clear();

            Menu.ShowHeader();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("This would be the window where you can write messages.");

            Console.ReadKey();
        }

        public void ConnectToOutputWindow()
        {
            listener.Start();

            TcpClient client = listener.AcceptTcpClient();

            NetworkStream stream = client.GetStream();

            if (stream.CanWrite && stream.CanRead)
            {
                string sessionData = NetworkManager.ReadMessage(stream, 48);
            }
        }
    }
}
