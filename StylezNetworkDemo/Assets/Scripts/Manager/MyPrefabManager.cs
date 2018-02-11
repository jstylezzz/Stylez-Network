/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StylezNetworkDemo.Manager
{
    /// <summary>
    ///
    /// </summary>
    public class MyPrefabManager : MonoBehaviour
    {
        public static MyPrefabManager Instance { get; private set; }
        private Dictionary<string, GameObject> m_prefabRegistry = new Dictionary<string, GameObject>();

        /// <summary>
        /// Script entry point.
        /// </summary>
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else return;

            GameObject[] ga = Resources.LoadAll<GameObject>(@"Prefabs\Objects");
            foreach(GameObject g in ga)
            {
                m_prefabRegistry.Add(g.name, g);
            }

            Debug.Log($"[INFO]: Loaded {ga.Length} prefabs into registry.");
        }

        public GameObject Get(string prefabName)
        {
            if (!m_prefabRegistry.ContainsKey(prefabName)) return null;
            else return m_prefabRegistry[prefabName];
        }
    }
}