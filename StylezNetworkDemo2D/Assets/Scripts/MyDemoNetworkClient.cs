/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/

using System.Collections;
using System;
using UnityEngine;
using System.Net.Sockets;
using System.Net;

/// <summary>
///
/// </summary>
public class MyDemoNetworkClient : MonoBehaviour
{
    [SerializeField]
    private string m_serverIP;

    [SerializeField]
    private int m_port;

    private Socket m_cSock;
    private IPAddress m_ipAddr;
    private IPEndPoint m_ipe;

	/// <summary>
	/// Script entry point.
	/// </summary>
	private void Start () 
	{
        m_cSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        if (IPAddress.TryParse(m_serverIP, out m_ipAddr))
        {
            try
            {
                m_ipe = new IPEndPoint(m_ipAddr, m_port);
                m_cSock.Connect(m_ipe);
            }
            catch(Exception e)
            {
                Debug.LogError("Could not create connection base. Error message:\n" + e.Message);
            }
        }
        else Debug.LogError("Could not parse the server IP.");
	}

    public void OnApplicationQuit()
    {
        if (m_cSock.Connected)
        {
            m_cSock.Shutdown(SocketShutdown.Both);
            Debug.Log("Disconnecting from server..");
        }

    }

    /// <summary>
    /// Script update look.
    /// </summary>
    private void Update () 
	{
		
	}
}
