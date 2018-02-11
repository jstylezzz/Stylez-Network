/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StylezNetworkShared.Game.World.Objects;

namespace StylezNetworkDemo.Network
{
    /// <summary>
    ///
    /// </summary>
    public class MyNetObjectManager
    {
        public static MyNetObjectManager Instance { get; private set; }

        private static Dictionary<int, MySyncedObject> m_syncedObjectDict = new Dictionary<int, MySyncedObject>();

        public MyNetObjectManager()
        {
            if (Instance == null) Instance = this;
            else return;
        }

        public bool Contains(int id)
        {
            return m_syncedObjectDict.ContainsKey(id);
        }

        public bool Contains(MySyncedObject so)
        {
            return m_syncedObjectDict.ContainsValue(so);
        }

        public MyWorldObject[] GetLocalMovementUpdateObjects()
        {
            List<MyWorldObject> so = new List<MyWorldObject>();
            foreach (KeyValuePair<int, MySyncedObject> kv in m_syncedObjectDict)
            {
                if (kv.Value.DataChangedLocally) so.Add(kv.Value.ExportWorldObjectData());
            }

            return so.ToArray();
        }

        /// <summary>
        /// Get the SyncedObject instance from the registry by ID.
        /// </summary>
        /// <param name="id">The ID of the SyncedObject.</param>
        /// <returns>SyncedObject instance or null.</returns>
        public MySyncedObject Get(int id)
        {
            if (!Contains(id)) return null;
            else return m_syncedObjectDict[id];
        }

        /// <summary>
        /// Get the SyncedObject ID from the instance.
        /// </summary>
        /// <param name="instance">The instance of the SyncedObject.</param>
        /// <returns>ID of SyncedObject or -1 if not exist</returns>
        public int Get(MySyncedObject instance)
        {
            if (!Contains(instance)) return -1;
            else return instance.ObjectID;
        }

        public Dictionary<int, MySyncedObject> GetAll()
        {
            return new Dictionary<int, MySyncedObject>(m_syncedObjectDict);
        }

        public int[] GetAllIDs()
        {
            if (m_syncedObjectDict.Count == 0) return null;

            int[] all = new int[m_syncedObjectDict.Count];
            int i = 0;

            foreach (KeyValuePair<int, MySyncedObject> kv in m_syncedObjectDict)
            {
                all[i] = kv.Key;
                i++;
            }

            return all;
        }

        public GameObject[] GetAllPhysicalObjects()
        {
            if (m_syncedObjectDict.Count == 0) return null;

            GameObject[] ga = new GameObject[m_syncedObjectDict.Count];
            int i = 0;
            foreach (KeyValuePair<int, MySyncedObject> kv in m_syncedObjectDict)
            {
                ga[i] = kv.Value.PhysicalObject;
                i++;
            }

            return ga;
        }

        public void Register(MySyncedObject instance)
        {
            if (!Contains(instance.ObjectID)) m_syncedObjectDict.Add(instance.ObjectID, instance);
            else Debug.LogWarning($"[WARN]: Object with ID {instance.ObjectID} already registered.");
        }

        public void Unregister(int id)
        {
            if (Contains(id)) m_syncedObjectDict.Remove(id);
            else Debug.LogWarning($"[WARN]: Object ID with ID {id} is not registered.");
        }

        public void Unregister(MySyncedObject instance)
        {
            if (Contains(instance.ObjectID)) m_syncedObjectDict.Remove(instance.ObjectID);
            else Debug.LogWarning($"[WARN]: Object ID with ID {instance.ObjectID} is not registered.");
        }
    }
}