/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/

using StylezNetworkDemo.Utiliy;
using StylezNetworkShared.Objects;
using System;
using UnityEngine;

namespace StylezNetworkDemo.Network
{
    /// <summary>
    ///
    /// </summary>
    public class MyNetworkedObject : MonoBehaviour
    {
        #region Inspector Variables

        [SerializeField]
        private UnityEngine.Object[] m_objectsToDeleteRemote;

        [SerializeField]
        private float m_maxDistanceForInterpolate;

        [SerializeField]
        private int m_interpolationMaxTimeSeconds;

        #endregion

        #region Variables

        private MyDynamicObject m_dynamicObject;
        private bool m_interpolating = false;
        private DateTime m_interpolateStartTime;
        private Vector3 m_interpolateStartPos;

        #endregion

        #region Properties

        public bool IsALocalObject { get; private set; }

        public MyDynamicObject DynamicObject { get { return m_dynamicObject; } }

        #endregion

        #region Lifecycle

        /// <summary>
        /// Script entry point.
        /// </summary>
        public virtual void Start()
        {
            
        }

        /// <summary>
        /// Script update loop.
        /// </summary>
        public virtual void Update()
        {
            if (m_interpolating) PerformInterpolation();
            else PerformMovement();
        }

        #endregion

        #region Public Methods

        public void SetupNetworkedObject(MyDynamicObject wo)
        {
            m_dynamicObject = wo;
            OwnershipCheck();
            MyNetObjectManager.Instance.Register(this);
        }

        public void UpdateNetworkedObject(MyDynamicObject wo)
        {
            
            if (IsALocalObject)
            {

            }
            else
            {
                Vector3 woPos = new Vector3(wo.PosX, wo.PosY, wo.PosZ);
                m_dynamicObject.UpdatePosition(woPos.x, woPos.y, woPos.z);

                if (Vector3.Distance(woPos, transform.position) > m_maxDistanceForInterpolate)
                {
                    transform.position = woPos;
                }
                else if(!m_interpolating)
                {
                    m_interpolateStartTime = DateTime.Now;
                    m_interpolateStartPos = transform.position;
                    m_interpolating = true;
                }
            }
        }

        //public void UpdateNetworkedObjectForces(MyMovementData md)
        //{
        //    m_dynamicObject.MovementData.UpdateMovement(md.XDirection, md.YDirection, md.ZDirection, md.Speed, md.IsMoving);
        //}

        //public void NullifyNetworkedObjectForces()
        //{
        //    m_dynamicObject.MovementData.StopAllMovement();
        //}

        public void DeleteObject()
        {
            MyNetObjectManager.Instance.Unregister(m_dynamicObject.ObjectID);
            Destroy(gameObject);
        }

        #endregion

        #region Private Methods

        private void OwnershipCheck()
        {
            if (MyOwnershipUtility.GetObjectOwner(m_dynamicObject) != EMyMultiplayerSide.SIDE_SELF)
            {
                foreach (UnityEngine.Object o in m_objectsToDeleteRemote) Destroy(o);
                IsALocalObject = false;
            }
            else IsALocalObject = true;
        }

        private void PerformInterpolation()
        {
            float delta = (DateTime.Now - m_interpolateStartTime).Seconds / m_interpolationMaxTimeSeconds;

            if (delta < 1)
            {
                transform.position = Vector3.Lerp(m_interpolateStartPos, new Vector3(m_dynamicObject.PosX, m_dynamicObject.PosY, m_dynamicObject.PosZ), delta);
            }
            else
            {
                m_interpolating = false;
                transform.position = new Vector3(m_dynamicObject.PosX, m_dynamicObject.PosY, m_dynamicObject.PosZ);
            }
        }

        private void PerformMovement()
        {
            //TODO
            //Fix movement with the new movement data from DynamicObject instance.

            //Vector3 woCurrent = new Vector3(m_dynamicObject.PosX, m_dynamicObject.PosY, m_dynamicObject.PosZ);
            //Vector3 towards = new Vector3(m_dynamicObject.x, m_dynamicObject.MovementData.YDirection, m_dynamicObject.MovementData.ZDirection) * m_dynamicObject.MovementData.Speed;
            //transform.Translate(towards);
            //m_dynamicObject.ObjectPosition.UpdatePosition(transform.position.x, transform.position.y, transform.position.z);
        }

        #endregion
    }
}