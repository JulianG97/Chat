using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace Server
{
    public class MessageManager
    {
        private TcpClient client;
        private UserAccountManager userAccountManager;

        public MessageManager(UserAccountManager userAccountManager, TcpClient client)
        {
            this.client = client;
            this.userAccountManager = userAccountManager;
        }

        public void ForwardMessagesToAllClients()
        {
            while (true)
            {
                if (client.Connected == true)
                {
                    NetworkStream clientStream = client.GetStream();

                    if (clientStream.DataAvailable == false)
                    {
                        Thread.Sleep(100);
                        continue;
                    }

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

                        Protocol message = ProtocolCreator.PublishMessage(messageProtocolContent[0], GetUserGroup(messageProtocolContent[0]), messageProtocolContent[1]);

                        foreach (User user in userAccountManager.OnlineUser)
                        {
                            NetworkManager.SendMessage(message, user.Client);
                        }
                    }

                    Thread.Sleep(100);
                }
                else
                {
                    break;
                }
            }
        }

        public char GetUserGroup(string username)
        {
            char userGroup = 'U';

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

                            if (userGroupArray[0] == 'U' && userGroupArray[1] == 's' && userGroupArray[2] == 'e' && userGroupArray[3] == 'r' && userGroupArray[4] == ' ' && userGroupArray[5] == 'G' && userGroupArray[6] == 'r' && userGroupArray[7] == 'o' && userGroupArray[8] == 'u' && userGroupArray[9] == 'p' && userGroupArray[10] == ':' && userGroupArray[11] == ' ')
                            {
                                string userGroupString = string.Empty;

                                for (int i = 12; i < userGroupArray.Length; i++)
                                {
                                    userGroupString = userGroupString + userGroupArray[i];
                                }

                                if (userGroupString == "Admin")
                                {
                                    userGroup = 'A';
                                }
                                else if (userGroupString == "Mod")
                                {
                                    userGroup = 'M';
                                }
                            }
                        }
                    }
                }
            }

            return userGroup;
        }
    }
}
