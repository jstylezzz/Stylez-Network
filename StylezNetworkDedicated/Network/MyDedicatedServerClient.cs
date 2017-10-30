﻿using System;
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
            SendMessage("TEST MET DIEREN!");
            m_aliveTimer.Start();
        }

        private void SendMessage(string text)
        {
            byte[] len = BitConverter.GetBytes(text.Length);
            byte[] messageBytes = Encoding.ASCII.GetBytes(text);
            byte[] combinedBytes = new byte[len.Length + messageBytes.Length];
            Buffer.BlockCopy(len, 0, combinedBytes, 0, 4);
            Buffer.BlockCopy(messageBytes, 0, combinedBytes, 4, messageBytes.Length);
            ClientSocket.Send(combinedBytes);
        }

        public void Disconnect()
        {
            m_aliveTimer.Stop();
            ClientSocket.Shutdown(SocketShutdown.Both);
            m_serverInstance.UnregisterClient(this);
        }

        private void PerformSocketAliveCheck(object sender, ElapsedEventArgs e)
        {
            if (PollSocketForConnection() == false) Disconnect();
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
