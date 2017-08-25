using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
    public class User
    {
        public string Username
        {
            get;
            set;
        }

        public TcpClient Client
        {
            get;
            set;
        }

        public string SessionKey
        {
            get;
            set;
        }
    }
}
