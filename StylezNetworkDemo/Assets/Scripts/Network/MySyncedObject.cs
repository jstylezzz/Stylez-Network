﻿/*
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
    public class MySyncedObject : MonoBehaviour
    {

        #region Properties

        public MyWorldObject WorldObject { get { return m_worldObjectInstance; } }

        public int ObjectID { get { return m_worldObjectInstance.ObjectID; } }

        public GameObject PhysicalObject { get { return gameObject; } }

        public bool DataChangedLocally
        {
            get
            {
                return (m_dataChangedLocally || m_worldObjectInstance.MovementData.IsMoving);
            }
            set { m_dataChangedLocally = false; }
        }

        #endregion

        #region Variables

        [SerializeField]
        private Object[] m_destroyOnRemote; 

        private MyWorldObject m_worldObjectInstance;
        private bool m_dataChangedLocally = false;

        #endregion

        #region Lifecycle

        /// <summary>
        /// Script entry point.
        /// </summary>
        private void Start()
        {
            if (Utiliy.MyOwnershipUtility.GetObjectOwner(m_worldObjectInstance) != Utiliy.EMyMultiplayerSide.SIDE_SELF) RemoveLocalObjects();
        }

        /// <summary>
        /// Script update loop.
        /// </summary>
        private void Update()
        {
            OnUpdate();
        }

        #endregion

        #region Public Methods

        public void InitializeObject(MyWorldObject wInst)
        {
            m_worldObjectInstance = wInst;
            MyNetObjectManager.Instance.Register(this);
        }

        public void UpdateWorldObjectInstance(MyWorldObject wo)
        {
            if (DataChangedLocally) return; //Only update if the locally changed data is already exported, or if there is none
            m_worldObjectInstance = wo;
            if(!m_worldObjectInstance.MovementData.IsMoving) transform.position = new Vector3(wo.ObjectPosition.x, wo.ObjectPosition.y, wo.ObjectPosition.z);
        }

        public void DeleteObject()
        {
            MyNetObjectManager.Instance.Unregister(ObjectID);
            Destroy(gameObject);
        }

        public void UpdateMovementLocal(Vector3 direction, float speed, float atTime)
        {
            m_worldObjectInstance.MovementData = new MyMovementData(direction.x, direction.y, direction.z, speed, atTime, (speed != 0));
            DataChangedLocally = true;
        }

        public MyMovementData ExportMovementData()
        {
            DataChangedLocally = false;
            return m_worldObjectInstance.MovementData;
        }

        public MySyncedObject ExportData()
        {
            DataChangedLocally = false;
            return this;
        }

        public MyWorldObject ExportWorldObjectData()
        {
            DataChangedLocally = false;
            return m_worldObjectInstance;
        }

        public void StoppedMoving()
        {
            m_worldObjectInstance.ObjectPosition = new MyWorldPosition(transform.position.x, transform.position.y, transform.position.z, m_worldObjectInstance.ObjectPosition.Dimension);
        }

        #endregion

        #region Protected Methods

        protected virtual void OnUpdate()
        {
            MyMovementData movData = m_worldObjectInstance.MovementData;
            if (movData.IsMoving)
            {
                transform.Translate((new Vector3(movData.XDirection, movData.YDirection, movData.ZDirection) * movData.Speed) * Time.deltaTime);
                m_worldObjectInstance.ObjectPosition = new MyWorldPosition(transform.position.x, transform.position.y, transform.position.z, m_worldObjectInstance.ObjectPosition.Dimension);
            }

            if(transform.position != new Vector3(m_worldObjectInstance.ObjectPosition.x, m_worldObjectInstance.ObjectPosition.y, m_worldObjectInstance.ObjectPosition.z))
            {
                m_worldObjectInstance.ObjectPosition = new MyWorldPosition(transform.position.x, transform.position.y, transform.position.z, m_worldObjectInstance.ObjectPosition.Dimension);
            }
        }

        #endregion

        #region Private Methods

        private void RemoveLocalObjects()
        {
            for (int i = 0, len = m_destroyOnRemote.Length; i < len; i++)
            {
                Destroy(m_destroyOnRemote[i]);
            }
        }

        #endregion
    }
}