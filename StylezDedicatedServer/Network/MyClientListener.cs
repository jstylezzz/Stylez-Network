using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using StylezNetworkShared.Logging;
using StylezDedicatedServer.Core;
using StylezNetworkShared.Network;

namespace StylezDedicatedServer.Network
{
    public class MyClientListener
    {
        private int m_port;
        private IPAddress m_ip;
        private MyWorkingSocket m_socketData;

        public MyClientListener(string ip, int port)
        {
            m_port = port;
            if(IPAddress.TryParse(ip, out m_ip))
            {
                MyLogger.LogInfo($"Starting server on {ip}:{port}.");
                SetupListener();
                StartListener();
            }
        }

        private void SetupListener()
        {
            m_socketData = new MyWorkingSocket();
            IPEndPoint ipe = new IPEndPoint(m_ip, m_port);
            m_socketData.SocketInstance = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            m_socketData.SocketInstance.Bind(ipe);
        }

        private void StartListener()
        {
            m_socketData.SocketInstance.Listen(10);
            m_socketData.SocketInstance.BeginAccept(new AsyncCallback(OnReceiveIncomingConnection), m_socketData.SocketInstance);
            MyLogger.LogInfo("Listener started.");
        }

        private void OnReceiveIncomingConnection(IAsyncResult ar)
        {
            MyNetworkClient nc = new MyNetworkClient(EMyNetClientMode.MODE_SERVER);
            nc.RegisterWorkingSocket(m_socketData.SocketInstance.EndAccept(ar));

            MyLogger.LogInfo($"Incoming connection from {nc.ClientEndpoint.ToString()}..");

            MyClientManager.Instance.RegisterClient(nc);
            m_socketData.SocketInstance.BeginAccept(new AsyncCallback(OnReceiveIncomingConnection), m_socketData.SocketInstance);
        }
    }
}
