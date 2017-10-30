using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using StylezNetworkDedicated.Manager;

namespace StylezNetworkDedicated.Network
{
    public class MyDedicatedServerBase
    {
        public IPAddress ServerIPAddress { get { return m_ip; } }
        private int Port { get { return m_port; } }

        private Dictionary<int, MyDedicatedServerClient> m_clientRegistry = new Dictionary<int, MyDedicatedServerClient>();
        private Stack<int> m_freeIDRegistry = new Stack<int>();
        private int m_highestClientID = 0;
        private IPAddress m_ip;
        private int m_port = 7777;
        private Socket m_serverSock;
        private EndPoint m_ePoint;

        public MyDedicatedServerBase(string ip, int port)
        {
            SetupNetworkBase(ip, port);
            StartDedicatedServer();
        }

        private void StartDedicatedServer()
        {
            try
            {
                m_serverSock.Listen(10);
                Console.WriteLine("[INFO]: Server started on " + m_ip.ToString() + ":" + m_port);
            }
            catch (Exception e)
            {
                Console.WriteLine("[FATAL ERROR]: Something went wrong while starting the server. Error message:\n" + e.Message);
            }
            MyServerEventManager.OnServerReady?.Invoke();
            m_serverSock.BeginAccept(new AsyncCallback(ReceiveIncomingConnection), m_serverSock);
        }

        private void ReceiveIncomingConnection(IAsyncResult ar)
        {
            Socket clientC = m_serverSock.EndAccept(ar);
            Console.WriteLine("[NEW]: Incoming connection from " + clientC.RemoteEndPoint.ToString());
            RegisterClient(clientC);
        }

        private void SetupNetworkBase(string ip, int port)
        {
            if (IPAddress.TryParse(ip, out m_ip))
            {
                try
                {
                    m_port = port;
                    m_ePoint = new IPEndPoint(m_ip, m_port);
                    m_serverSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    m_serverSock.Bind(m_ePoint);
                    Console.WriteLine("[INFO]: Server ready to start on " + m_ip.ToString() + ":" + m_port);
                }
                catch(Exception e)
                {
                    Console.WriteLine("[FATAL ERROR]: Something went wrong while setting up the network base. Error message:\n" + e.Message);
                }
            }
            else Console.WriteLine("[FATAL ERROR]: Could not parse IP address.");
        }

        private void RegisterClient(Socket s)
        {
            int freeID = GetFirstFreeClientID(true);
            m_clientRegistry.Add(freeID, new MyDedicatedServerClient(s, freeID, this));
        }

        public void UnregisterClient(MyDedicatedServerClient c)
        {
            m_freeIDRegistry.Push(c.ClientID);
            m_clientRegistry.Remove(c.ClientID);
            Console.WriteLine("[LEAVE]: Client " + c.ClientID + " has left the server.");
        }

        public int GetFirstFreeClientID(bool removeOld = false)
        {
            if (removeOld)
            {
                if (m_freeIDRegistry.Count == 0)
                {
                    m_highestClientID++;
                    return m_highestClientID;
                }
                else return m_freeIDRegistry.Pop();
            }
            else
            {
                if (m_freeIDRegistry.Count == 0) return m_highestClientID + 1;
                else return m_freeIDRegistry.Peek();
            }
        }
    }
}
