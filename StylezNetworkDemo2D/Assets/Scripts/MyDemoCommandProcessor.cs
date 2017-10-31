/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/

using System;
using System.Collections.Generic;
using UnityEngine;
using StylezNetwork.Commands;
using StylezNetwork.Objects;

/// <summary>
///
/// </summary>
public class MyDemoCommandProcessor : MonoBehaviour 
{
    private MyDemoNetworkClient m_netClient;

	/// <summary>
	/// Script entry point.
	/// </summary>
	private void Start () 
	{
        m_netClient = FindObjectOfType<MyDemoNetworkClient>();
	}


    public void ProcessCommand(int cmdId, string cmdJson)
    {
        switch ((EMyNetworkCommand)Enum.Parse(typeof(EMyNetworkCommand), cmdId.ToString()))
        {
            case EMyNetworkCommand.COMMAND_AUTH:
            {
                MyAuthCommand authCmd = JsonUtility.FromJson<MyAuthCommand>(cmdJson);
                
                m_netClient.CompleteAuthentication(authCmd.AuthCode, authCmd.ClientID);
                break;
            }
            case EMyNetworkCommand.COMMAND_WORLD_GETOBJECTS:
            {
                MyRequestAllObjectsCommand cmd = JsonUtility.FromJson<MyRequestAllObjectsCommand>(cmdJson);
                for (int i = 0; i < cmd.WorldObjectLocations.Length; i++)
                {
                    
                    MyMainThreadPump.Instance().Enqueue(() =>
                    {
                        GameObject g = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                        g.transform.position = new Vector3((float)cmd.WorldObjectLocations[0].x, (float)cmd.WorldObjectLocations[0].y, (float)cmd.WorldObjectLocations[0].z);
                    });
                }
                break;
            }
        }
    }
}
