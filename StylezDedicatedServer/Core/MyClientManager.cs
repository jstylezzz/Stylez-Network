﻿using System;
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
        public static MyClientManager Instance { get; private set; }
        public Dictionary<int, MyNetworkClient> ClientRegistry { get { return m_clientRegistry; } }
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
            m_clientRegistry.Add(GetFreeClientID(), c);
            c.OnTransmissionReceived += OnMessageReceived;
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
                return m_highestClientID--;
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
