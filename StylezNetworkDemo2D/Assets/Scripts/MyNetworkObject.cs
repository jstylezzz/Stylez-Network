/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StylezNetwork.Commands;
using StylezNetwork.MathEx;
using StylezNetwork.Objects;

/// <summary>
///
/// </summary>
public class MyNetworkObject : MonoBehaviour
{
    public int ObjectID { get; private set; }
    public Vector3 Position { get { return transform.position; } }
    public int Dimension { get; private set; }

    private MyWorldObjectManager m_worldMan;

	/// <summary>
	/// Script entry point.
	/// </summary>
	private void Start () 
	{
        
    }

    public void SetupNetworkObject(int id, int dimension)
    {
        m_worldMan = MyWorldObjectManager.Instance;
        this.ObjectID = id;
        this.Dimension = dimension;
        m_worldMan.RegisterObject(this);
    }
	
    public void UpdatePosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

	/// <summary>
	/// Script update look.
	/// </summary>
	private void Update () 
	{
		
	}

    private void OnDestroy()
    {
        m_worldMan.UnregisterWorldObject(this);   
    }
}
