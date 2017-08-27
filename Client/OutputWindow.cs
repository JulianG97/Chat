using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class OutputWindow
    {
        public void Start(string username, string sessionKey)
        {
            Console.Clear();

            Menu.ShowHeader();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("This would be the window where you receive messages.");

            Console.ReadKey();
        }

        public void ConnectToInputWindow()
        {

        }
    }
}
