using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace StylezNetworkDedicated.Network
{
    public class MyDedicatedServerClient
    {
        public int ClientID { get; private set; }
        public Socket ClientSocket { get; private set; }

        public MyDedicatedServerClient(Socket s, int id)
        {
            this.ClientID = id;
            this.ClientSocket = s;
        }
    }
}
