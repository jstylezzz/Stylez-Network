/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StylezNetworkDemo.Network;
using StylezNetworkShared.Network;

namespace StylezNetworkDemo.Player
{
    /// <summary>
    ///
    /// </summary>
    public class MyBasicPlayerMovement : MonoBehaviour
    {
        [SerializeField]
        private float m_speed;

        private MySyncedObject m_syncedObject;
        private Vector4 m_cachedKeyStates = Vector4.zero;

        /// <summary>
        /// Script entry point.
        /// </summary>
        private void Start()
        {
            m_syncedObject = GetComponent<MySyncedObject>();
        }

        /// <summary>
        /// Script update loop.
        /// </summary>
        private void Update()
        {
            Vector3 md = Vector3.zero;
            bool stateChanged = false;

            if(Input.GetKeyDown(KeyCode.W))
            {
                md.y = m_speed;
                m_cachedKeyStates.x = 1;
                stateChanged = true;
            }
            else if(Input.GetKeyUp(KeyCode.W))
            {
                m_cachedKeyStates.x = 0;
                stateChanged = true;
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                md.y = -m_speed;
                m_cachedKeyStates.y = 1;
                stateChanged = true;
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                m_cachedKeyStates.y = 0;
                stateChanged = true;
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                md.x = -m_speed;
                m_cachedKeyStates.z = 1;
                stateChanged = true;
            }
            else if (Input.GetKeyUp(KeyCode.A))
            {
                m_cachedKeyStates.z = 0;
                stateChanged = true;
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                md.x = m_speed;
                m_cachedKeyStates.w = 1;
                stateChanged = true;
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                m_cachedKeyStates.w = 0;
                stateChanged = true;
            }

            if(stateChanged)
            {
                //If still moving, a direction change has occured. Update.
                if(IsStillMoving())
                {
                    m_syncedObject.UpdateMovementLocal(md, m_speed, Time.time);
                }
                else //Not moving anymore, send stop signal.
                {
                    m_syncedObject.StoppedMoving();
                    m_syncedObject.UpdateMovementLocal(Vector3.zero, 0, Time.time);
                }
            }
        }

        private bool IsStillMoving()
        {
            if (m_cachedKeyStates.x != 0 || m_cachedKeyStates.y != 0 || m_cachedKeyStates.z != 0 || m_cachedKeyStates.w != 0) return true;
            else return false;
        }
    }
}