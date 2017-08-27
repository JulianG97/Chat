using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 0)
            {
                if (args[0] == "\\OpenInputWindow")
                {
                    InputWindow inputWindow = new InputWindow();
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
