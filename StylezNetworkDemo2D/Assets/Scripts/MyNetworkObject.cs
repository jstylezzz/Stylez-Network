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
        MyCreateObjectCommand cmd = new MyCreateObjectCommand(new Vector3Simple(transform.position.x - 5, transform.position.y, transform.position.z), 0);
        MyDemoNetworkClient.Instance.EnqueueMessage(JsonUtility.ToJson(cmd), (int)EMyNetworkCommand.COMMAND_OBJECT_CREATE);

        MyRequestAllObjectsCommand cmdc = new MyRequestAllObjectsCommand(new Vector3Simple(transform.position.x, transform.position.y, transform.position.z), 0, 50);
        MyDemoNetworkClient.Instance.EnqueueMessage(JsonUtility.ToJson(cmdc), (int)EMyNetworkCommand.COMMAND_WORLD_GETOBJECTS);
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
