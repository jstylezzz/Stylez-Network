using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Timers;

namespace StylezNetworkDedicated.Network
{
    public class MyDedicatedServerClient
    {
        public int ClientID { get; private set; }
        public Socket ClientSocket { get; private set; }

        public const int SocketAliveCheckRate = 4000; //Time in milliseconds

        private Timer m_aliveTimer;
        private MyDedicatedServerBase m_serverInstance;

        public MyDedicatedServerClient(Socket s, int id, MyDedicatedServerBase sInstance)
        {
            m_aliveTimer = new Timer(SocketAliveCheckRate);
            m_aliveTimer.Elapsed += PerformSocketAliveCheck;

            this.ClientID = id;
            this.ClientSocket = s;
            m_serverInstance = sInstance;
        }

        private void PerformSocketAliveCheck(object sender, ElapsedEventArgs e)
        {
            if(PollSocketForConnection() == false)
            {
                ClientSocket.Dispose();
                m_serverInstance.UnregisterClient(this);
            }
        }

        private bool PollSocketForConnection()
        {
            try
            {
                return !(ClientSocket.Poll(1, SelectMode.SelectRead) && ClientSocket.Available == 0);
            }
            catch (SocketException) { return false; }
        }
    }
}
