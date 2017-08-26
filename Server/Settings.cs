using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Server
{
    public class Settings
    {
        public int ServerPort
        {
            get;
            set;
        }

        public string ServerUserPath
        {
            get;
            set;
        }

        public void CheckConfigFile()
        {
            int port;
            string userPath;

            string configPath = Directory.GetCurrentDirectory();
            configPath = configPath + @"\config.txt";

            if (File.Exists(configPath) == true)
            {
                string[] configContent = File.ReadAllLines(configPath);

                this.CheckConfigPort(configContent[0], out port);

                if (port == 0)
                {
                    this.GenerateDefaultConfigFile();
                }
                else if (port != 0)
                {
                    this.CheckConfigUserPath(configContent[1], out userPath);

                    if (userPath == string.Empty)
                    {
                        this.GenerateDefaultConfigFile();
                    }
                    else if (userPath != string.Empty)
                    {
                        ServerPort = port;
                        ServerUserPath = userPath;
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

            configFile[0] = "Port: 80";
            configFile[1] = "User Path: " + configPath + @"\User";

            File.WriteAllLines(configPath + @"\config.txt", configFile);

            if (Directory.Exists(configPath + @"\User") == false)
            {
                Directory.CreateDirectory(configPath + @"\User");
            }

            this.ServerPort = 80;
            this.ServerUserPath = configPath + @"\User";
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

        public void CheckConfigUserPath(string userPathLine, out string userPath)
        {
            char[] userPathArray = userPathLine.ToCharArray();
            userPath = string.Empty;

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
                }
            }
        }
    }
}
