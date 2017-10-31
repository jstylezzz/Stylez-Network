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

/// <summary>
///
/// </summary>
public class MyNetworkObject : MonoBehaviour 
{

	/// <summary>
	/// Script entry point.
	/// </summary>
	private void Start () 
	{
        MyCreateObjectCommand cmd = new MyCreateObjectCommand(new Vector3Simple(transform.position.x, transform.position.y, transform.position.z), 0);
        MyDemoNetworkClient.Instance.SendMessage(JsonUtility.ToJson(cmd), EMyNetworkCommand.COMMAND_OBJECT_CREATE);
	}
	
	/// <summary>
	/// Script update look.
	/// </summary>
	private void Update () 
	{
		
	}

    private void OnDestroy()
    {
        
    }
}
