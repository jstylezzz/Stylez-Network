using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StylezNetworkShared.Commands;
using StylezNetworkShared.Logging;
using StylezNetworkShared.Network;

namespace StylezDedicatedServer.Core
{
    public class MyClientManager
    {
        /// <summary>
        /// Class wide random instance
        /// </summary>
        private static Random RandInstance = new Random();

        /// <summary>
        /// The single possible instance of the ClientManager.
        /// </summary>
        public static MyClientManager Instance { get; private set; }

        /// <summary>
        /// The dictionary containing the client registry.
        /// </summary>
        public Dictionary<int, MyNetworkClient> ClientRegistry { get { return m_clientRegistry; } }

        /// <summary>
        /// Event that is fired when one of the clients receives a transmission.
        /// </summary>
        public event OnTransmissionReceivedDelegate OnTransmissionReceive;

        private Dictionary<int, MyNetworkClient> m_clientRegistry = new Dictionary<int, MyNetworkClient>();
        private int m_highestClientID = -1;
        private Stack<int> m_freeIDS = new Stack<int>();
        

        public MyClientManager()
        {
            if (Instance == null) Instance = this;
            else MyLogger.LogError("There is already a client manager created. Instance dropped.");
        }

        /// <summary>
        /// Register a client to the client registry.
        /// </summary>
        /// <param name="c">The NetClient instance to register.</param>
        public void RegisterClient(MyNetworkClient c)
        {
            int ID = GetFreeClientID();
            c.OnTransmissionReceived += OnMessageReceived;
            c.OnDisconnect += ClientDisconnectHandler;
            c.AuthClient(ID, GenerateAuthCode());
            m_clientRegistry.Add(ID, c);
            MyLogger.LogInfo($"Client ID {ID} has connected.");
        }

        /// <summary>
        /// Generate a random string to be used as auth code.
        /// </summary>
        /// <returns>Random string, 8 characters long.</returns>
        private string GenerateAuthCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 8)
              .Select(s => s[RandInstance.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Called when a client disconnects.
        /// </summary>
        /// <param name="clientid">The ID of the disconnecting client.</param>
        private void ClientDisconnectHandler(int clientid)
        {
            MyNetworkClient c = m_clientRegistry[clientid];
            c.OnTransmissionReceived -= OnMessageReceived;
            c.OnDisconnect -= ClientDisconnectHandler;
            c.SetAuthenticated(false);
            UnregisterClient(clientid);
            MyLogger.LogInfo($"Client ID {clientid} has disconnected.");
        }

        /// <summary>
        /// Unregisters a client from the client registry.
        /// </summary>
        /// <param name="clientID">The client ID to unregister.</param>
        public void UnregisterClient(int clientID)
        {
            m_clientRegistry.Remove(clientID);
            ReturnClientIDToPool(clientID);
        }

        /// <summary>
        /// Place a client ID in the pool of available client ID's.
        /// </summary>
        /// <param name="id">The ID to place in the pool.</param>
        private void ReturnClientIDToPool(int id)
        {
            m_freeIDS.Push(id);
        }

        /// <summary>
        /// Send a transmission to connected clients.
        /// </summary>
        /// <param name="cmd">The NetCommand that should be sent to the client.</param>
        public void SendTransmissionToClients(MyNetCommand cmd)
        {
            foreach(KeyValuePair<int, MyNetworkClient> c in m_clientRegistry) c.Value.SendTransmission(cmd);
        }

        /// <summary>
        /// Send a transmission to connected clients, with exception of the specified client ID.
        /// </summary>
        /// <param name="cmd">The NetCommand that should be sent to the client.</param>
        /// <param name="clientIDException">The client ID that should not receive this message.</param>
        public void SendTransmissionToClients(MyNetCommand cmd, int clientIDException)
        {
            foreach (KeyValuePair<int, MyNetworkClient> c in m_clientRegistry)
            {
                if (c.Key != clientIDException) c.Value.SendTransmission(cmd);
            }
        }

        /// <summary>
        /// Get a free client ID. If the ID pool has a free ID that can be re-used, that one
        /// will be used. No free ID's? Then a new client ID will be picked (highest existing + 1).
        /// </summary>
        /// <param name="markInUse">True to mark this ID in use (remove it from the available possibilities), false to let the ID be available.</param>
        /// <returns></returns>
        private int GetFreeClientID(bool markInUse = true)
        {
            if(m_freeIDS.Count == 0)
            {
                if (markInUse) m_highestClientID++;
                return m_highestClientID;
            }
            else
            {
                if (markInUse) return m_freeIDS.Pop();
                else return m_freeIDS.Peek();
            }
        }

        /// <summary>
        /// Called when a client receives a message.
        /// </summary>
        /// <param name="fromClient">The instance of the receiving client.</param>
        /// <param name="mes">The message that was received.</param>
        private void OnMessageReceived(MyNetworkClient fromClient, MyNetCommand mes)
        {
            //We forward the event to potential 'external' code.
            //The game specific code from the StylezDedicatedServer.Game namespace for example.
            OnTransmissionReceive?.Invoke(fromClient, mes);
        }
    }
}
