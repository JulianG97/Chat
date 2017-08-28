using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    public static class NetworkManager
    {
        public static void SendMessage(Protocol protocol, TcpClient client)
        {
            if (client.Connected == true)
            {
                NetworkStream stream = client.GetStream();

                byte[] sendBuffer = protocol.Create();

                if (client.Connected == true)
                {
                    stream.Write(sendBuffer, 0, sendBuffer.Length);
                }
            }
        }

        public static string ReadMessage(TcpClient client, int messageLength)
        {
            NetworkStream stream = client.GetStream();

            byte[] receiveBuffer = new byte[messageLength];

            int receivedBytes = stream.Read(receiveBuffer, 0, receiveBuffer.Length);

            string message = Encoding.ASCII.GetString(receiveBuffer, 0, receivedBytes);

            return message;
        }
    }
}
