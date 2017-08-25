using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    public class NetworkManager
    {
        public static void Connect(IPEndPoint ip, TcpClient client)
        {
            try
            {
                client.Connect(ip);

                NetworkStream stream = client.GetStream();

                if (!stream.CanRead || !stream.CanWrite)
                {
                    throw new Exception();
                }
            }
            catch
            {
                throw new ArgumentException("The server is unreachable! Please try again later!");
            }
        }

        public static void SendMessage(Protocol protocol, NetworkStream stream)
        {
            byte[] sendBuffer = protocol.Create();

            stream.Write(sendBuffer, 0, sendBuffer.Length);
        }

        public static string ReadMessage(NetworkStream stream, int messageLength)
        {
            byte[] receiveBuffer = new byte[messageLength];

            int receivedBytes = stream.Read(receiveBuffer, 0, receiveBuffer.Length);

            string message = Encoding.ASCII.GetString(receiveBuffer, 0, receivedBytes);

            return message;
        }
    }
}
