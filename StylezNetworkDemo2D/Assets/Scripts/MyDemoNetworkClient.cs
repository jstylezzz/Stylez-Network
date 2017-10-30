/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/

using System.Text;
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

    private byte[] m_streamBuffer;

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
                ReceiveFromBegin();
            }
            catch(Exception e)
            {
                Debug.LogError("Could not create connection base. Error message:\n" + e.Message);
            }
        }
        else Debug.LogError("Could not parse the server IP.");
	}

    private void ReceiveFromBegin()
    {
        m_streamBuffer = new byte[4];
        m_cSock.BeginReceive(m_streamBuffer, 0, 4, SocketFlags.None, new AsyncCallback(OnBufferLengthReceived), m_cSock);
    }

    private void OnBufferLengthReceived(IAsyncResult ar)
    {
        int messageLength = BitConverter.ToInt32(m_streamBuffer, 0);
        Debug.Log("LEN RECEIVED: " + messageLength);
        m_streamBuffer = new byte[messageLength];
        m_cSock.BeginReceive(m_streamBuffer, 0, messageLength, SocketFlags.None, new AsyncCallback(OnMessageReceived), m_cSock);
    }

    private void OnMessageReceived(IAsyncResult ar)
    {
        m_cSock.EndReceive(ar);
        ReceiveFromBegin();
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
