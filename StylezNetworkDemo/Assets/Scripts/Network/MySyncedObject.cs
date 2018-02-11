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
    public class MySyncedObject : MonoBehaviour
    {

        #region Properties

        public MyWorldObject WorldObject { get { return m_worldObjectInstance; } }

        public int ObjectID { get { return m_worldObjectInstance.ObjectID; } }

        public GameObject PhysicalObject { get { return gameObject; } }

        #endregion

        #region Variables

        private MyWorldObject m_worldObjectInstance;

        #endregion

        #region Lifecycle

        /// <summary>
        /// Script entry point.
        /// </summary>
        private void Start()
        {

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
            m_worldObjectInstance = wo;
        }

        public void DeleteObject()
        {
            MyNetObjectManager.Instance.Unregister(ObjectID);
            Destroy(gameObject);
        }

        #endregion

        #region Protected Methods

        protected virtual void OnUpdate()
        {
            MyMovementData movData = m_worldObjectInstance.MovementData;
            if (movData.IsMoving)
            {
                transform.Translate((new Vector3(movData.XDirection, movData.YDirection, movData.ZDirection) * movData.Speed) * Time.deltaTime);
            }
        }

        #endregion  
    }
}