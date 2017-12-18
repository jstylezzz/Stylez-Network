using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StylezDedicatedServer.Network;
using StylezNetworkShared.Logging;
using StylezNetworkShared.Network;

namespace StylezDedicatedServer.Core
{
    public class MyClientManager
    {
        public static MyClientManager Instance { get; private set; }
        public Dictionary<int, MyNetworkClient> ClientRegistry { get { return m_clientRegistry; } }

        private Dictionary<int, MyNetworkClient> m_clientRegistry = new Dictionary<int, MyNetworkClient>();
        private int m_highestClientID = -1;
        private Stack<int> m_freeIDS = new Stack<int>();

        public MyClientManager()
        {
            if (Instance == null) Instance = this;
            else MyLogger.LogError("There is already a client manager created. Instance dropped.");
        }

        public void RegisterClient(MyNetworkClient c)
        {
            m_clientRegistry.Add(GetFreeClientID(), c);
            c.OnTransmissionReceived += OnMessageReceived;
        }

        public void UnregisterClient(int clientID)
        {
            m_clientRegistry.Remove(clientID);
            ReturnClientIDToPool(clientID);
        }

        private void ReturnClientIDToPool(int id)
        {
            m_freeIDS.Push(id);
        }

        private int GetFreeClientID(bool markInUse = true)
        {
            if(m_freeIDS.Count == 0)
            {
                if (markInUse) m_highestClientID++;
                return m_highestClientID--;
            }
            else
            {
                if (markInUse) return m_freeIDS.Pop();
                else return m_freeIDS.Peek();
            }
        }

        private void OnMessageReceived(string mes)
        {
            
        }
    }
}
