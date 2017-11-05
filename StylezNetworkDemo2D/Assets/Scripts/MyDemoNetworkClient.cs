/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/

using System.Text;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using UnityEngine.SceneManagement;
using StylezNetwork.Commands;
using StylezNetwork.MathEx;
using System.Collections;

/// <summary>
///
/// </summary>
public class MyDemoNetworkClient : MonoBehaviour
{
    public static MyDemoNetworkClient Instance { get; private set; }
    public const int TransmissionPerQuarterSecond = 4;

    [SerializeField]
    private string m_serverIP;

    [SerializeField]
    private int m_port;

    [SerializeField]
    private double m_streamDist;

    private Socket m_cSock;
    private IPAddress m_ipAddr;
    private IPEndPoint m_ipe;

    private byte[] m_streamBuffer;
    private int m_activeBufferLength;

    private int m_clientID;
    private string m_token;
    private MyDemoCommandProcessor m_cmdProcessor;
    private Queue<byte[]> m_sendQueue = new Queue<byte[]>();

    private bool m_sendActive = false;
    private Coroutine m_sendRoutine;

    /// <summary>
    /// Script entry point.
    /// </summary>
    private void Start()
    {
        Instance = this;
        m_cmdProcessor = FindObjectOfType<MyDemoCommandProcessor>();
        m_cSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        if (IPAddress.TryParse(m_serverIP, out m_ipAddr))
        {
            try
            {
                m_ipe = new IPEndPoint(m_ipAddr, m_port);
                m_cSock.Connect(m_ipe);
                ReceiveFromBegin();
            }
            catch (Exception e)
            {
                Debug.LogError("Could not create connection base. Error message:\n" + e.Message);
            }
        }
        else Debug.LogError("Could not parse the server IP. " + gameObject.name);
    }

    private IEnumerator SendRoutine()
    {   
        yield return new WaitForSeconds(0.25f);
        ProcessSendQueue();
        if (m_sendActive) m_sendRoutine = StartCoroutine(SendRoutine());
    }

    private void ProcessSendQueue()
    {
        try
        {
            List<byte[]> standardCmds = new List<byte[]>();
            standardCmds.Add(GetMessageBytes(JsonUtility.ToJson(new MyAreaUpdateCommand(m_streamDist, 0, new Vector3Simple(transform.position.x, transform.position.y, transform.position.z))), (int)EMyNetworkCommand.COMMAND_REQUEST_AREAUPDATE));
            int transmissionsLeft = ((m_sendQueue.Count + standardCmds.Count > TransmissionPerQuarterSecond) ? TransmissionPerQuarterSecond : m_sendQueue.Count + standardCmds.Count);

            for (int i = 0; i < standardCmds.Count; i++)
            {
                m_cSock.Send(standardCmds[i]);
                transmissionsLeft--;
            }

            for (int i = 0; i < transmissionsLeft; i++)
            {
                byte[] b = m_sendQueue.Dequeue();
                m_cSock.Send(b);
            }
        }
        catch
        {
            Disconnect();
        }
    }

    private void ReceiveFromBegin()
    {
        m_streamBuffer = new byte[4];
        m_cSock.BeginReceive(m_streamBuffer, 0, 4, SocketFlags.None, new AsyncCallback(OnBufferLengthReceived), m_cSock);
    }

    private void OnBufferLengthReceived(IAsyncResult ar)
    {
        int messageLength = BitConverter.ToInt32(m_streamBuffer, 0);
        m_streamBuffer = new byte[messageLength];
        m_cSock.BeginReceive(m_streamBuffer, 0, messageLength, SocketFlags.None, new AsyncCallback(OnMessageReceived), m_cSock);
    }

    private void OnMessageReceived(IAsyncResult ar)
    {
        m_cSock.EndReceive(ar);
        byte[] cmdidBytes = new byte[4];
        byte[] cmdBytes = new byte[m_streamBuffer.Length - 4];
        Buffer.BlockCopy(m_streamBuffer, 0, cmdidBytes, 0, 4);
        Buffer.BlockCopy(m_streamBuffer, 4, cmdBytes, 0, m_streamBuffer.Length - 4);
        string cmd = Encoding.ASCII.GetString(cmdBytes);
        m_cmdProcessor.ProcessCommand(BitConverter.ToInt32(cmdidBytes, 0), cmd);
        ReceiveFromBegin();
    }

    public void CompleteAuthentication(string token, int cid)
    {
        m_clientID = cid;
        m_token = token;

        MyCrossThreadOperator.Instance.Enqueue(() => 
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            m_sendActive = true;
            m_sendRoutine = StartCoroutine(SendRoutine());
        });
        
    }

    public void EnqueueMessage(string text, int commandid)
    {
        byte[] cmdId = BitConverter.GetBytes(commandid); //4 bytes
        byte[] len = BitConverter.GetBytes(text.Length + cmdId.Length); //4 bytes

        byte[] messageBytes = Encoding.ASCII.GetBytes(text); //? bytes
        byte[] combinedBytes = new byte[len.Length + cmdId.Length + messageBytes.Length];
        Buffer.BlockCopy(len, 0, combinedBytes, 0, 4);
        Buffer.BlockCopy(cmdId, 0, combinedBytes, 4, cmdId.Length);
        Buffer.BlockCopy(messageBytes, 0, combinedBytes, 8, messageBytes.Length);
        m_sendQueue.Enqueue(combinedBytes);
    }

    public byte[] GetMessageBytes(string text, int commandid)
    {
        byte[] cmdId = BitConverter.GetBytes(commandid); //4 bytes
        byte[] len = BitConverter.GetBytes(text.Length + cmdId.Length); //4 bytes

        byte[] messageBytes = Encoding.ASCII.GetBytes(text); //? bytes
        byte[] combinedBytes = new byte[len.Length + cmdId.Length + messageBytes.Length];
        Buffer.BlockCopy(len, 0, combinedBytes, 0, 4);
        Buffer.BlockCopy(cmdId, 0, combinedBytes, 4, cmdId.Length);
        Buffer.BlockCopy(messageBytes, 0, combinedBytes, 8, messageBytes.Length);
        return combinedBytes;
    }

    private void Disconnect()
    {
        if (m_cSock.Connected)
        {
            m_sendActive = false;
            MyCrossThreadOperator.Instance.Enqueue(() => { StopCoroutine(m_sendRoutine); });
            m_cSock.Shutdown(SocketShutdown.Both);
            Debug.Log("Disconnecting from server..");
        }
    }

    public void OnApplicationQuit()
    {
        Disconnect();

    }
}
