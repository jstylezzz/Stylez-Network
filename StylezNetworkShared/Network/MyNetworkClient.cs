using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using StylezNetworkShared.Commands;
using StylezNetworkShared.Logging;
using System.Threading;

namespace StylezNetworkShared.Network
{
    public delegate void OnTransmissionReceivedDelegate(MyNetworkClient fromClient, MyNetCommand message);
    public delegate void OnClientDisconnectDelegate(int clientID);

    public class MyNetworkClient
    {
        public delegate void OnConnectedToServerDelegate(bool success);
        
        /// <summary>
        /// NetClient mode. Is this instance a client or server?
        /// </summary>
        public EMyNetClientMode NetClientMode { get { return m_netClientMode; } }

        public EndPoint ClientEndpoint { get { return m_sockData.SocketInstance.RemoteEndPoint; } }

        /// <summary>
        /// The WorkingSocket instance of this client.
        /// </summary>
        public MyWorkingSocket WorkingSocket { get { return m_sockData; } }

        /// <summary>
        /// The client's authcode.
        /// </summary>
        public string AuthCode { get { return m_authCode; } }

        /// <summary>
        /// Is the client authenticated?
        /// </summary>
        public bool IsAuthenticated { get { return m_isAuthenticated; } }

        /// <summary>
        /// Event that is triggered when finished connecting to a server.
        /// </summary>
        public event OnConnectedToServerDelegate OnConnectedToServer;

        /// <summary>
        /// Event that is called when this client receives a transmission.
        /// </summary>
        public event OnTransmissionReceivedDelegate OnTransmissionReceived;

        /// <summary>
        /// Called when disconnecting from server/when being disconnected from server.
        /// </summary>
        public event OnClientDisconnectDelegate OnDisconnect;

        /// <summary>
        /// Is the socket connected?
        /// </summary>
        public bool IsConnected { get { return !((m_sockData.SocketInstance.Poll(1000, SelectMode.SelectRead) && (m_sockData.SocketInstance.Available == 0)) || !m_sockData.SocketInstance.Connected); } }

        /// <summary>
        /// This client's ID.
        /// </summary>
        public int ClientID { get { return m_clientID; } }

        /// <summary>
        /// Amount of milliseconds to wait before checking if the socket's connection is alive.
        /// </summary>
        public int AliveCheckTimeout { get; set; } = 4000;

        private MyWorkingSocket m_sockData = new MyWorkingSocket();
        private EMyNetClientMode m_netClientMode;
        private int m_clientID = -1;
        private string m_authCode = "00000000";
        private bool m_isAuthenticated = false;
        private bool m_shuttingDown = false;
        private Thread m_connectionCheckThread;

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
            m_connectionCheckThread = new Thread(ThreadedConnectionCheckLoop);
            m_connectionCheckThread.Start();
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
                m_connectionCheckThread = new Thread(ThreadedConnectionCheckLoop);
                m_connectionCheckThread.Start();
            }
        }

        /// <summary>
        /// A loop that checks whether the connection is still alive.
        /// This runs from a separate thread, and will automatically stop
        /// when the client disconnects.
        /// </summary>
        private void ThreadedConnectionCheckLoop()
        {
            while(m_shuttingDown == false)
            {
                //We check is m_shuttingDown is equal to false since the other thread might have set it to 
                //true after the while loop started.
                if (!IsConnected && m_shuttingDown == false) Disconnect();
                else Thread.Sleep(AliveCheckTimeout);
            }
        }

        /// <summary>
        /// Called when transmission is sent to server.
        /// </summary>
        private void OnTransmissionSent(IAsyncResult ar)
        {
            try
            {
                m_sockData.SocketInstance.EndSend(ar);
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(SocketException))
                {
                    if (((SocketException)e).SocketErrorCode == SocketError.Shutdown && m_shuttingDown == false) Disconnect();
                }
            }
            if (!IsConnected && m_shuttingDown == false) Disconnect();
        }

        /// <summary>
        /// Start the listening routine on the active working socket.
        /// Note, this can be done in both client and server mode.
        /// </summary>
        private void ListenRoutineStart()
        {
            m_sockData.Buffer = new byte[MyNetPacketUtil.InitialTransmissionLength];
            m_sockData.ReceivedAuthCode = "";
            m_sockData.TransmissionLength = 0;

            try
            {
                m_sockData.SocketInstance.BeginReceive(m_sockData.Buffer, 0, MyNetPacketUtil.InitialTransmissionLength, SocketFlags.None, new AsyncCallback(OnTransmissionDataReceived), m_sockData.SocketInstance);
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(SocketException))
                {
                    if (((SocketException)e).SocketErrorCode == SocketError.Shutdown && m_shuttingDown == false) Disconnect();
                }
            }
        }

        /// <summary>
        /// Called when the active working socket receives transmission information.
        /// This information includes the total transmission length and the authcode.
        /// </summary>
        private void OnTransmissionDataReceived(IAsyncResult ar)
        {
            try
            { m_sockData.SocketInstance.EndReceive(ar);
                m_sockData.TransmissionLength = MyNetPacketUtil.GetTransmissionLength(m_sockData.Buffer);
                m_sockData.ReceivedAuthCode = MyNetPacketUtil.GetAuthCodeFromBuf(m_sockData.Buffer);
                m_sockData.Buffer = new byte[m_sockData.TransmissionLength];
                m_sockData.SocketInstance.BeginReceive(m_sockData.Buffer, 0, m_sockData.TransmissionLength, SocketFlags.None, new AsyncCallback(OnTransmissionContentReceived), m_sockData.SocketInstance);
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(SocketException))
                {
                    if (((SocketException)e).SocketErrorCode == SocketError.Shutdown && m_shuttingDown == false) Disconnect();
                }
            }
        }

        /// <summary>
        /// Called when the active working socket receives transmission content.
        /// This is essentially the command string.
        /// </summary>
        private void OnTransmissionContentReceived(IAsyncResult ar)
        {
            try
            {
                m_sockData.SocketInstance.EndReceive(ar);

                MyNetCommand content = MyNetPacketUtil.GetCommandFromBuf(m_sockData.Buffer);
                content.AuthCode = m_sockData.ReceivedAuthCode; //IMPORTANT! Set the authcode this command came with.

                OnTransmissionReceived?.Invoke(this, content);
                ListenRoutineStart();
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(SocketException))
                {
                    if (((SocketException)e).SocketErrorCode == SocketError.Shutdown && m_shuttingDown == false) Disconnect();
                }
            }
        }

        /// <summary>
        /// Send a transmission to the other end of the socket.
        /// </summary>
        /// <param name="message">The NetCommand to send.</param>
        public void SendTransmission(MyNetCommand cmd)
        {
            MyNetPacket p = MyNetPacketUtil.PackMessage(cmd.CommandID, cmd.CommandJSON, m_authCode);
            try
            {
                m_sockData.SocketInstance.BeginSend(p.Transmission, 0, p.TransmissionLength, SocketFlags.None, new AsyncCallback(OnTransmissionSent), m_sockData.SocketInstance);
            }
            catch(Exception e)
            {
                if(e.GetType() == typeof(SocketException))
                {
                    if (((SocketException)e).SocketErrorCode == SocketError.Shutdown && m_shuttingDown == false) Disconnect();
                }
            }
        }

        /// <summary>
        /// Authenticate this client instance.
        /// </summary>
        /// <param name="ID">The ID to set.</param>
        /// <param name="code">The auth code to register.</param>
        public void AuthClient(int ID, string code)
        {
            m_clientID = ID;
            m_authCode = code;
        }

        /// <summary>
        /// Set the auth status for this client.
        /// </summary>
        /// <param name="set">The auth status.</param>
        public void SetAuthenticated(bool set)
        {
            m_isAuthenticated = set;
        }

        /// <summary>
        /// Disconnect this client.
        /// </summary>
        public void Disconnect()
        {
            if (m_shuttingDown) return;
            m_shuttingDown = true;
            m_sockData.SocketInstance.Shutdown(SocketShutdown.Both);
            OnDisconnect?.Invoke(m_clientID);
        }
    }

    /// <summary>
    /// The available NetClient modes.
    /// </summary>
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
