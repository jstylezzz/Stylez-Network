/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/

using StylezNetworkDemo.Network;
using UnityEngine;

namespace StylezNetworkDemo.Player
{
    /// <summary>
    ///
    /// </summary>
    public class MyBasicPlayerMovement : MonoBehaviour
    {
        [SerializeField]
        private float m_speed;

        private MyNetworkedObject m_syncedObject;
        private Vector2 m_currentMovement = Vector2.zero;
        private Vector2 m_cachedMovement = Vector2.zero;
        private float m_moveStartTime;

        private bool m_jumping = false;
        private int m_jumpFrames = 0;

        /// <summary>
        /// Script entry point.
        /// </summary>
        private void Start()
        {
            m_syncedObject = GetComponent<MyNetworkedObject>();
        }

        /// <summary>
        /// Script update loop.
        /// </summary>
        private void Update()
        {
            if (Input.GetKey(KeyCode.D)) m_currentMovement.x= m_speed;
            else if (Input.GetKey(KeyCode.A)) m_currentMovement.x = -m_speed;
            else m_currentMovement.x = 0;

            if (Input.GetKeyDown(KeyCode.Space) && m_jumping == false)
            {
                m_jumping = true;
                m_jumpFrames = 60 * 2;
                GetComponent<Rigidbody2D>().gravityScale = 0;
            }

            if (m_jumping && m_jumpFrames > 0)
            {
                m_currentMovement.y = m_speed;
                m_jumpFrames--;
            }
            else if(m_jumping && m_jumpFrames == 0)
            {
                m_jumping = false;
                m_currentMovement.y = 0;
                GetComponent<Rigidbody2D>().gravityScale = 1;
            }

            if (m_currentMovement != m_cachedMovement)
            {
                //If still moving, a direction change has occured. Update.
                if(IsStillMoving())
                {
                    if (m_moveStartTime == -1) m_moveStartTime = Time.time;
                    m_syncedObject.UpdateNetworkedObjectForces(new MyMovementData(m_currentMovement.x, m_currentMovement.y, 0, m_speed, m_moveStartTime, true));
                }
                else //Not moving anymore, send stop signal.
                {
                    m_moveStartTime = -1;
                    m_syncedObject.NullifyNetworkedObjectForces();
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