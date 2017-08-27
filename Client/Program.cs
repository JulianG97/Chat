using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 0)
            {
                IPAddress serverIp;
                int serverPort;

                if (args[0] == "OpenInputWindow" && IPAddress.TryParse(args[1], out serverIp) == true && int.TryParse(args[2], out serverPort) == true)
                {
                    InputWindow inputWindow = new InputWindow(serverIp, serverPort);
                    inputWindow.Start();
                }
            }
            else
            {
                Menu.DisplayMainMenu();
            }
        }
    }
}
