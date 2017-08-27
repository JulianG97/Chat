using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Server
{
    public class MessageManager
    {
        private List<User> onlineUser;
        private TcpClient client;

        public MessageManager(List<User> onlineUser, TcpClient client)
        {
            this.onlineUser = onlineUser;
            this.client = client;
        }

        public void ForwardMessagesToAllClients()
        {
            while (true)
            {
                string messageProtocol = NetworkManager.ReadMessage(client, 302);

                char[] mesageProtocolArray = messageProtocol.ToCharArray();

                if (messageProtocol[0] == 'C' && messageProtocol[1] == 'H' && messageProtocol[2] == 'A' && messageProtocol[3] == 'T' && messageProtocol[4] == 'M' && messageProtocol[5] == 'E')
                {
                    string messageString = string.Empty;

                    for (int i = 6; i < mesageProtocolArray.Length; i++)
                    {
                        messageString = messageString + mesageProtocolArray[i];
                    }

                    string[] messageProtocolContent = messageString.Split('-');

                    Protocol message = ProtocolCreator.PublishMessage(messageProtocolContent[0], userGroup, messageProtocolContent[1]);
                    NetworkManager.SendMessage(message, client);
                }
            }
        }

        public void GetUserGroup(string username)
        {
            string configPath = Directory.GetCurrentDirectory();
            configPath = configPath + @"\config.txt";

            if (File.Exists(configPath))
            {
                string[] configContent = File.ReadAllLines(configPath);

                char[] userPathArray = configContent[1].ToCharArray();
                string userPath = string.Empty;

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

                        string path = userPath + @"\" + username + ".txt";

                        if (File.Exists(path))
                        {
                            string[] userFileArray = File.ReadAllLines(path);

                            char[] userGroupArray = userFileArray[2].ToCharArray();

                            if (userGroupArray[0] == 'U' && userGroupArray[1] == 's' && userGroupArray[2] == 'e' && userGroupArray[3] == 'r' && userGroupArray[4] == ' ' && userGroupArray[5] == 'G' && userGroupArray[6] == 'r' && userGroupArray[7] == 'o' && userGroupArray[8] == 'u' && userGroupArray[9] == 'p')
                        }
                    }
                }
            }
        }
    }
}
