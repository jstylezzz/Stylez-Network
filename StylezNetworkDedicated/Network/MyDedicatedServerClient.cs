﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Timers;
using StylezNetwork.Commands;

namespace StylezNetworkDedicated.Network
{
    public class MyDedicatedServerClient
    {
        public int ClientID { get; private set; }
        public Socket ClientSocket { get; private set; }

        public const int SocketAliveCheckRate = 4000; //Time in milliseconds
        public const int AuthTokenLength = 5;

        private bool m_isAuthenticated = false;
        private string m_authToken = null;

        private Timer m_aliveTimer;
        private MyDedicatedServerBase m_serverInstance;

        private byte[] m_streamBuffer;

        public MyDedicatedServerClient(Socket s, int id, MyDedicatedServerBase sInstance)
        {
            m_aliveTimer = new Timer(SocketAliveCheckRate);
            m_aliveTimer.Elapsed += PerformSocketAliveCheck;

            this.ClientID = id;
            this.ClientSocket = s;
            m_serverInstance = sInstance;
            GenerateAuthToken();
            SendAuthRequest();
            m_aliveTimer.Start();
            Console.WriteLine("[JOIN]: Client with ID " + ClientID + " has joined the server.");
        }

        private void SendAuthRequest()
        {
            SendMessage(JsonConvert.SerializeObject(new MyAuthCommand(m_authToken, ClientID)), (int)EMyNetworkCommand.COMMAND_AUTH);
            StartReceiving();
        }

        private void StartReceiving()
        {
            m_streamBuffer = new byte[4];
            ClientSocket.BeginReceive(m_streamBuffer, 0, 4, SocketFlags.None, new AsyncCallback(OnMessageLengthReceived), ClientSocket);
        }

        private void OnMessageLengthReceived(IAsyncResult ar)
        {
            (ar.AsyncState as Socket).EndReceive(ar);
            int len = BitConverter.ToInt32(m_streamBuffer, 0);
            m_streamBuffer = new byte[len];
            ClientSocket.BeginReceive(m_streamBuffer, 0, len, SocketFlags.None, new AsyncCallback(OnMessageReceived), ClientSocket);
        }

        private void OnMessageReceived(IAsyncResult ar)
        {
            try
            {
                byte[] cmdIdBytes = new byte[4];
                byte[] commandBytes = new byte[m_streamBuffer.Length - 4];
                Buffer.BlockCopy(m_streamBuffer, 0, cmdIdBytes, 0, 4);
                Buffer.BlockCopy(m_streamBuffer, 4, commandBytes, 0, commandBytes.Length);
                int cmdID = BitConverter.ToInt32(cmdIdBytes, 0);
                string command = Encoding.ASCII.GetString(commandBytes);
                Program.Instance.CommandProcessorInstance.ProcessCommand(cmdID, command);
            }
            catch
            {
                Disconnect();
            }
        }

        public void GenerateAuthToken()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            m_authToken = new string(Enumerable.Repeat(chars, AuthTokenLength)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void SendMessage(string text, int commandid)
        {
            byte[] cmdId = BitConverter.GetBytes(commandid); //4 bytes
            byte[] len = BitConverter.GetBytes(text.Length + cmdId.Length); //4 bytes

            byte[] messageBytes = Encoding.ASCII.GetBytes(text); //? bytes
            byte[] combinedBytes = new byte[len.Length + cmdId.Length + messageBytes.Length];
            Buffer.BlockCopy(len, 0, combinedBytes, 0, 4);
            Buffer.BlockCopy(cmdId, 0, combinedBytes, 4, cmdId.Length);
            Buffer.BlockCopy(messageBytes, 0, combinedBytes, 8, messageBytes.Length);
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
