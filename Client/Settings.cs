using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace Client
{
    public class Settings
    {
        public int ServerPort
        {
            get;
            set;
        }

        public IPAddress ServerIP
        {
            get;
            set;
        }

        public void CheckConfigFile()
        {
            IPAddress ip;
            int port;

            string configPath = Directory.GetCurrentDirectory();
            configPath = configPath + @"\config.txt";

            if (File.Exists(configPath) == true)
            {
                string[] configContent = File.ReadAllLines(configPath);

                this.CheckConfigIP(configContent[0], out ip);

                if (ip == IPAddress.Parse("0.0.0.0"))
                {
                    this.GenerateDefaultConfigFile();
                }
                else if (ip != IPAddress.Parse("0.0.0.0"))
                {
                    this.CheckConfigPort(configContent[1], out port);

                    if (port == 0)
                    {
                        this.GenerateDefaultConfigFile();
                    }
                    else if (port != 0)
                    {
                        this.ServerPort = port;
                        this.ServerIP = ip;
                    }
                }
            }
            else if (File.Exists(configPath) == false)
            {
                this.GenerateDefaultConfigFile();
            }
        }

        public void GenerateDefaultConfigFile()
        {
            string configPath = Directory.GetCurrentDirectory();

            string[] configFile = new string[2];

            configFile[0] = "IP: 10.0.0.7";
            configFile[1] = "Port: 80";

            File.WriteAllLines(configPath + @"\config.txt", configFile);

            this.ServerIP = IPAddress.Parse("10.0.0.7");
            this.ServerPort = 80;
        }

        public void CheckConfigIP(string ipLine, out IPAddress ip)
        {
            char[] ipArray = ipLine.ToCharArray();
            ip = IPAddress.Parse("0.0.0.0");

            if (ipArray[0] == 'I' && ipArray[1] == 'P' && ipArray[2] == ':' && ipArray[3] == ' ')
            {
                string ipString = string.Empty;

                for (int i = 4; i < ipArray.Length; i++)
                {
                    ipString = ipString + ipArray[i];
                }

                IPAddress.TryParse(ipString, out ip);
            }
        }

        public void CheckConfigPort(string portLine, out int port)
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
    }
}
