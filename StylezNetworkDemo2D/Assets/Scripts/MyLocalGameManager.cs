/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class MyLocalGameManager : MonoBehaviour 
{
    private MyWorldObjectManager m_oman;

	/// <summary>
	/// Script entry point.
	/// </summary>
	private void Start () 
	{
        m_oman = MyWorldObjectManager.Instance;
        MyWorldObjectManager.CreateObject(Vector3.zero, 0, StylezNetwork.Objects.EMyObjectType.OBJECT_PLAYER);
	}
}
