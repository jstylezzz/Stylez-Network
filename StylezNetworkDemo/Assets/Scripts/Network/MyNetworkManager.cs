/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StylezNetworkShared.Network;
using StylezNetworkShared.Commands;
using StylezNetworkShared.Game.Commands;
using System.Threading;
using StylezNetworkShared.Game.World.Objects;
using Newtonsoft.Json;

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

        private Queue<Action> m_actionList = new Queue<Action>();
        private static Queue<MyNetCommand> m_netCommands = new Queue<MyNetCommand>();

        private List<GameObject> m_objectList = new List<GameObject>();

        private bool m_performUpdates = false;

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
            m_netCommands.Enqueue(message);
            //Debug.Log($"RECEIVED {message.CommandID}: " + message.CommandJSON);
        }

        private void PerformAreaUpdate(MyAreaUpdate u)
        {
            if (u.WorldObjects == null) return;
            //Empty list for objects we keep
            List<GameObject> newObjects = new List<GameObject>();

            //List copied from the current object list
            List<GameObject> deleteObjects = new List<GameObject>(m_objectList);

            //Loop through all the objects in range. 
            //Objects that are in range are removed from the deleteObjects list.
            //We will be left with a list consisting of objects that are no longer in range.
            foreach (MyWorldObject p in u.WorldObjects)
            {
                GameObject g = GameObject.CreatePrimitive(PrimitiveType.Cube);
                g.transform.position = new Vector3(p.ObjectPosition.x, p.ObjectPosition.y, p.ObjectPosition.z);
                newObjects.Add(g);
                deleteObjects.Remove(g);
            }

            //Now we delete those objects
            foreach (GameObject g in deleteObjects) Destroy(g);
            m_objectList = newObjects; //And assign our updated list as active objects list.
        }

        /// <summary>
        /// Called when disconnecting from the server.
        /// </summary>
        private void OnDisconnect()
        {
            m_performUpdates = false;
            m_netClient.OnDisconnectFromServer -= OnDisconnect;
            Debug.LogWarning("Disconnected from the server.");
        }

        /// <summary>
        /// Be sure to disconnect on destroy. Otherwise things might
        /// stay active in the background.
        /// </summary>
        private void OnDestroy()
        {
            m_performUpdates = false;
            if (m_netClient.IsConnected) m_netClient.Disconnect();
        }

        private void SendAreaUpdate()
        {
            while(m_performUpdates)
            {
                m_netClient.SendTransmission(new MyNetCommand((int)EMyNetworkCommands.WORLD_AREA_UPDATE));
                Thread.Sleep(1000);
                if (!m_performUpdates) break;
            }
        }

        private void ProcessCommandsAndActions()
        {
            if (m_actionList.Count > 0) m_actionList.Dequeue()?.Invoke();
            if (m_netCommands.Count > 0)
            {
                MyNetCommand message = m_netCommands.Dequeue();
                switch ((EMyNetworkCommands)message.CommandID)
                {
                    case EMyNetworkCommands.AUTHENTICATE:
                    {
                        MyClientAuthCommand cmd = JsonUtility.FromJson<MyClientAuthCommand>(message.CommandJSON);
                        m_netClient.AuthClient(cmd.ClientID, cmd.AuthCode);
                        m_netClient.SetAuthenticated(true);
                        m_performUpdates = true;
                        new Thread(SendAreaUpdate).Start();
                        m_netClient.SendTransmission(new MyNetCommand((int)EMyNetworkCommands.SPAWN_OBJECT, JsonConvert.SerializeObject(new MyWorldObject(5, 10, 0, "PlayerObject", true))));
                        break;
                    }
                    case EMyNetworkCommands.WORLD_AREA_UPDATE:
                    { 
                        PerformAreaUpdate(JsonConvert.DeserializeObject<MyAreaUpdate>(message.CommandJSON));
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Script update loop.
        /// </summary>
        private void Update()
        {
            ProcessCommandsAndActions();
        }
    }
}