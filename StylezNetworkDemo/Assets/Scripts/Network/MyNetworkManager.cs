/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StylezNetworkShared.Network;
using StylezNetworkShared.Commands;
using StylezNetworkShared.Game.Commands;
using System.Threading;

namespace StylezNetworkDemo.Network
{
    /// <summary>
    ///
    /// </summary>
    public class MyNetworkManager : MonoBehaviour
    {
        [SerializeField]
        private string m_ip = "127.0.0.1";

        [SerializeField]
        private int m_port = 7788;

        private MyNetworkClient m_netClient;

        /// <summary>
        /// Script entry point.
        /// </summary>
        private void Start()
        {
            Debug.Log($"Starting the NetClient on {m_ip}:{m_port}.");
            m_netClient = new MyNetworkClient(EMyNetClientMode.MODE_CLIENTSIDE);
            m_netClient.OnConnectedToServer += OnServerConnectComplete;
            m_netClient.ConnectToServer(m_ip, m_port);
        }

        /// <summary>
        /// Called when server connection completes.
        /// </summary>
        /// <param name="success">Will be true when connection is successful, false if not.</param>
        private void OnServerConnectComplete(bool success)
        {
            if (success)
            {
                Debug.Log("Connection successful.");
                m_netClient.OnConnectedToServer -= OnServerConnectComplete;
                m_netClient.OnDisconnectFromServer += OnDisconnect;
                m_netClient.OnTransmissionReceivedClient += OnTransmissionReceived;
                
                m_netClient.SendTransmission(new StylezNetworkShared.Commands.MyNetCommand((int)EMyNetworkCommands.AUTHENTICATE));
            }
            else Debug.LogWarning("Connection to the server has failed.");
        }

        /// <summary>
        /// Called when receiving a transmission from the server.
        /// </summary>
        /// <param name="message">The NetCommand received.</param>
        private void OnTransmissionReceived(MyNetCommand message)
        {
            switch ((EMyNetworkCommands)message.CommandID)
            {
                case EMyNetworkCommands.AUTHENTICATE:
                {
                    MyClientAuthCommand cmd = JsonUtility.FromJson<MyClientAuthCommand>(message.CommandJSON);
                    m_netClient.AuthClient(cmd.ClientID, cmd.AuthCode);
                    m_netClient.SetAuthenticated(true);
                    break;
                }
            }
            //Debug.Log($"RECEIVED {message.CommandID}: " + message.CommandJSON);
        }

        /// <summary>
        /// Called when disconnecting from the server.
        /// </summary>

        private void OnDisconnect()
        {
            m_netClient.OnDisconnectFromServer -= OnDisconnect;
            Debug.LogWarning("Disconnected from the server.");
        }

        /// <summary>
        /// Be sure to disconnect on destroy. Otherwise things might
        /// stay active in the background.
        /// </summary>
        private void OnDestroy()
        {
            if (m_netClient.IsConnected) m_netClient.Disconnect();
        }

        /// <summary>
        /// Script update loop.
        /// </summary>
        private void Update()
        {

        }
    }
}