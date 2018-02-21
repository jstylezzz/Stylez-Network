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
using StylezNetworkDemo.Manager;

namespace StylezNetworkDemo.Network
{
    /// <summary>
    ///
    /// </summary>
    public class MyNetworkManager : MonoBehaviour
    {
        public static MyNetworkManager Instance { get; private set; }
        public MyNetworkClient NetClient { get { return m_netClient; } }
        public float PingInMS { get; private set; } = 999f;

        [SerializeField]
        private string m_ip = "127.0.0.1";

        [SerializeField]
        private int m_port = 7788;

        private MyNetworkClient m_netClient;

        private Queue<Action> m_actionList = new Queue<Action>();
        private static Queue<MyNetCommand> m_netCommands = new Queue<MyNetCommand>();

        private bool m_performUpdates = false;
        private bool m_areaUpdateAnswered = true;

        private MyNetObjectManager m_netObjectManager = new MyNetObjectManager();

        private Vector3 m_camPos;

        private bool m_waitingForPing = false;
        private DateTime m_pingSent;

        /// <summary>
        /// Script entry point.
        /// </summary>
        private void Start()
        {
            if (Instance == null) Instance = this;
            else return;

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
                
                m_netClient.SendTransmission(new MyNetCommand((int)EMyNetworkCommands.AUTHENTICATE));
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

            Dictionary<int, MySyncedObject> existingObjects = m_netObjectManager.GetAll();

            //List copied from the current object list
            Dictionary<int, MySyncedObject> deleteObjects = new Dictionary<int, MySyncedObject>(existingObjects);

            GameObject g;
            MySyncedObject so;
            //Loop through all the objects in range. 
            //Objects that are in range are removed from the deleteObjects list.
            //We will be left with a list consisting of objects that are no longer in range.
            foreach (MyWorldObject p in u.WorldObjects)
            {
                bool exists = m_netObjectManager.Contains(p.ObjectID);

                //New object. Add SyncedObject component to it and initialize it.
                if (exists == false)
                {
                    g = Instantiate(MyPrefabManager.Instance.Get(p.ObjectPrefabName));
                    g.transform.position = new Vector3(p.ObjectPosition.x, p.ObjectPosition.y, p.ObjectPosition.z);
                    so = g.GetComponent<MySyncedObject>();
                    so.InitializeObject(p);
                }
                else //Exists, remove it from the objects to delete
                {
                    existingObjects[p.ObjectID].UpdateWorldObjectInstance(p);
                    deleteObjects.Remove(p.ObjectID);
                }
            }

            //Now we delete those objects
            foreach (KeyValuePair<int, MySyncedObject> kv in deleteObjects)
            {
                kv.Value.DeleteObject();
            }
        }

        /// <summary>
        /// Called when disconnecting from the server.
        /// </summary>
        private void OnDisconnect()
        {
            m_performUpdates = false;
            m_netClient.OnDisconnectFromServer -= OnDisconnect;
            m_netClient.OnTransmissionReceivedClient -= OnTransmissionReceived;
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

        private void SendAreaUpdateRequest()
        {
            while(m_performUpdates)
            {
                if (m_areaUpdateAnswered == true)
                {

                    m_netClient.SendTransmission(new MyNetCommand((int)EMyNetworkCommands.WORLD_AREA_UPDATE, JsonConvert.SerializeObject(new MyAreaUpdateRequest(m_netClient.ClientID, m_camPos.x, m_camPos.y, 0, 0, 10))));
                    m_areaUpdateAnswered = false;
                }

                if (!m_waitingForPing) DoPingCheck();
                Thread.Sleep(300);
                if (!m_performUpdates) break;
            }
        }

        private void DoPingCheck()
        {
            m_waitingForPing = true;
            m_pingSent = DateTime.Now;
            m_netClient.SendTransmission(new MyNetCommand((int)EMyNetworkCommands.PERFORM_PING, " "));
        }

        private void SendAreaUpdate()
        {
            MyWorldObject[] updates;

            while (m_performUpdates)
            {
                updates = MyNetObjectManager.Instance.GetLocalObjectsForUpdate();
                if(updates.Length > 0)
                {
                    m_netClient.SendTransmission(new MyNetCommand((int)EMyNetworkCommands.MAKE_WORLD_AREA_UPDATE, JsonConvert.SerializeObject(new MyAreaUpdate(updates.Length, updates))));
                }
                Thread.Sleep(250);
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
                        new Thread(SendAreaUpdateRequest).Start();
                        new Thread(SendAreaUpdate).Start();
                        m_netClient.SendTransmission(new MyNetCommand((int)EMyNetworkCommands.SPAWN_OBJECT, JsonConvert.SerializeObject(new MyWorldObject(0, 1, 0, "PlayerObject", true))));
                        break;
                    }
                    case EMyNetworkCommands.WORLD_AREA_UPDATE:
                    {
                        m_areaUpdateAnswered = true;
                        PerformAreaUpdate(JsonConvert.DeserializeObject<MyAreaUpdate>(message.CommandJSON));
                        break;
                    }
                    case EMyNetworkCommands.PERFORM_PING:
                    {
                        PingInMS = (DateTime.Now - m_pingSent).Milliseconds;
                        m_waitingForPing = false;
                        break;
                    }
                }
            }
        }

        private void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 100, 100), $"Ping: {PingInMS} ms");
        }

        /// <summary>
        /// Script update loop.
        /// </summary>
        private void Update()
        {
            m_camPos = (Camera.main == null) ? Vector3.zero : Camera.main.transform.root.position;
            ProcessCommandsAndActions();
        }
    }
}