using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using StylezNetworkShared.Network;
using StylezNetworkShared.Logging;

namespace StylezNetworkShared.Network
{
    public class MyNetworkClient
    {
        public delegate void OnConnectedToServerDelegate(bool success);
        public delegate void OnTransmissionReceivedDelegate(string message);

        public EMyNetClientMode NetClientMode { get { return m_netClientMode; } }
        public EndPoint ClientEndpoint { get { return m_sockData.SocketInstance.RemoteEndPoint; } }
        public event OnConnectedToServerDelegate OnConnectedToServer;
        public event OnTransmissionReceivedDelegate OnTransmissionReceived;

        private MyWorkingSocket m_sockData = new MyWorkingSocket();
        private EMyNetClientMode m_netClientMode;

        /// <summary>
        /// Create a new instance of the NetworkClient.
        /// </summary>
        /// <param name="mode">The mode to run the network client in.</param>
        public MyNetworkClient(EMyNetClientMode mode)
        {
            m_netClientMode = mode;
        }

        /// <summary>
        /// Register an open socket as the working socket for this NetClient instance.
        /// Note, this can only be done while the NetClient is running in server mode.
        /// </summary>
        /// <param name="cSock">The socket to register as working socket.</param>
        public void RegisterWorkingSocket(Socket cSock)
        {
            if(m_netClientMode == EMyNetClientMode.MODE_CLIENT)
            {
                MyLogger.LogError("Cannot register a listen socket to a NetClient while it's running in client mode.");
                return;
            }

            m_sockData.SocketInstance = cSock;
            ListenRoutineStart();
        }

        /// <summary>
        /// Connect to a server.
        /// Note, this can only be done while the NetClient is running in client mode.
        /// </summary>
        /// <param name="ip">The IP to connect to.</param>
        /// <param name="port">The port to connect to.</param>
        public void ConnectToServer(string ip, int port)
        {
            if (m_netClientMode == EMyNetClientMode.MODE_SERVER)
            {
                MyLogger.LogError("Cannot connect NetClient to a server while it's running in server mode.");
                return;
            }

            IPAddress ipAdrr;
            if (IPAddress.TryParse(ip, out ipAdrr))
            {
                m_sockData.SocketInstance = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                m_sockData.SocketInstance.BeginConnect(new IPEndPoint(ipAdrr, port), new AsyncCallback(OnConnectedToServerCallback), m_sockData.SocketInstance);
            }
        }

        /// <summary>
        /// Called when the NetClient finishes connecting to a server.
        /// </summary>
        private void OnConnectedToServerCallback(IAsyncResult ar)
        {
            OnConnectedToServer?.Invoke(m_sockData.SocketInstance.Connected); //Fire the server connected event.
            if (m_sockData.SocketInstance.Connected)
            {
                ListenRoutineStart(); //Start the listen routine if connection was successful.
            }
        }

        /// <summary>
        /// Called when transmission is sent to server.
        /// </summary>
        private void OnTransmissionSent(IAsyncResult ar)
        {
            m_sockData.SocketInstance.EndSend(ar);
        }

        /// <summary>
        /// Start the listening routine on the active working socket.
        /// Note, this can be done in both client and server mode.
        /// </summary>
        private void ListenRoutineStart()
        {
            m_sockData.Buffer = new byte[12];
            m_sockData.ReceivedAuthCode = "";
            m_sockData.TransmissionLength = 0;
            m_sockData.SocketInstance.BeginReceive(m_sockData.Buffer, 0, 12, SocketFlags.None, new AsyncCallback(OnTransmissionDataReceived), m_sockData.SocketInstance);
        }

        /// <summary>
        /// Called when the active working socket receives transmission information.
        /// This information includes the total transmission length and the authcode.
        /// </summary>
        private void OnTransmissionDataReceived(IAsyncResult ar)
        {
            m_sockData.SocketInstance.EndReceive(ar);
            m_sockData.TransmissionLength = MyNetPacketUtil.GetTransmissionLength(m_sockData.Buffer);
            m_sockData.ReceivedAuthCode = MyNetPacketUtil.GetAuthCodeFromBuf(m_sockData.Buffer);

            m_sockData.Buffer = new byte[m_sockData.TransmissionLength];
            m_sockData.SocketInstance.BeginReceive(m_sockData.Buffer, 0, m_sockData.TransmissionLength, SocketFlags.None, new AsyncCallback(OnTransmissionContentReceived), m_sockData.SocketInstance);
        }

        /// <summary>
        /// Called when the active working socket receives transmission content.
        /// This is essentially the command string.
        /// </summary>
        private void OnTransmissionContentReceived(IAsyncResult ar)
        {
            m_sockData.SocketInstance.EndReceive(ar);

            string content = MyNetPacketUtil.GetMessageFromBuf(m_sockData.Buffer);
            OnTransmissionReceived?.Invoke(content);
            ListenRoutineStart();
        }

        /// <summary>
        /// Send a transmission to the other end of the socket.
        /// </summary>
        /// <param name="message">The message to send.</param>
        public void SendTransmission(string message)
        {
            MyNetPacket p = MyNetPacketUtil.PackMessage(message, "DOL69!!");
            m_sockData.SocketInstance.BeginSend(p.Transmission, 0, p.TransmissionLength, SocketFlags.None, new AsyncCallback(OnTransmissionSent), m_sockData.SocketInstance);
        }
    }

    
    public enum EMyNetClientMode
    {
        /// <summary>
        /// NetClient runs in server mode. 
        /// </summary>
        MODE_SERVER,

        /// <summary>
        /// NetClient runs in client mode.
        /// </summary>
        MODE_CLIENT
    }
}
