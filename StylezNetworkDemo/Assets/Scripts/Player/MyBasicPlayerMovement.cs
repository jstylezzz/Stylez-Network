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
        private Vector2 m_currentMovement = Vector2.zero;
        private Vector2 m_cachedMovement = Vector2.zero;

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
            if (Input.GetKey(KeyCode.W)) m_currentMovement.y = m_speed;
            else if (Input.GetKey(KeyCode.S)) m_currentMovement.y = -m_speed;
            else m_currentMovement.y = 0;

            if (Input.GetKey(KeyCode.D)) m_currentMovement.x= m_speed;
            else if (Input.GetKey(KeyCode.A)) m_currentMovement.x = -m_speed;
            else m_currentMovement.x = 0;
            Debug.Log(Input.GetKey(KeyCode.D));
            if (m_currentMovement != m_cachedMovement)
            {
                //If still moving, a direction change has occured. Update.
                if(IsStillMoving())
                {
                    m_syncedObject.UpdateMovementLocal(m_currentMovement, m_speed, Time.time);
                }
                else //Not moving anymore, send stop signal.
                {
                    m_syncedObject.StoppedMoving();
                    m_syncedObject.UpdateMovementLocal(Vector3.zero, 0, Time.time);
                }
            }
            m_cachedMovement = m_currentMovement;
        }

        private bool IsStillMoving()
        {
            if (m_currentMovement.sqrMagnitude != 0) return true;
            else return false;
        }
    }
}