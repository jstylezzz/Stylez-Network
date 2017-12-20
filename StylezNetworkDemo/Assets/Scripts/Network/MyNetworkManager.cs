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
        private int m_port = 7778;

        private MyNetworkClient m_netClient;

        /// <summary>
        /// Script entry point.
        /// </summary>
        private void Start()
        {
            Debug.Log($"Starting the NetClient on {m_ip}:{m_port}.");
            m_netClient = new MyNetworkClient(EMyNetClientMode.MODE_CLIENT);
            m_netClient.OnConnectedToServer += OnServerConnectComplete;
            m_netClient.ConnectToServer(m_ip, m_port);
        }

        private void OnServerConnectComplete(bool success)
        {
            if (success)
            {
                Debug.Log("Connection successful.");
                m_netClient.OnConnectedToServer -= OnServerConnectComplete;
                m_netClient.OnDisconnect += OnDisconnect;
            }
            else Debug.LogWarning("Connection to the server has failed.");
        }


        private void OnDisconnect(int clientID)
        {
            m_netClient.OnDisconnect -= OnDisconnect;
            Debug.LogWarning("Disconnected from the server.");
        }

        private void OnDestroy()
        {
            if (m_netClient.IsConnected) m_netClient.Disconnect();
        }

        /// <summary>
        /// Script update look.
        /// </summary>
        private void Update()
        {

        }
    }
}