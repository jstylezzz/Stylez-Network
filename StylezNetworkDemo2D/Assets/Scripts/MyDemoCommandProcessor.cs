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
        Debug.Log("CMD ID " + cmdId + " CMD " + cmdJson);
        switch ((EMyNetworkCommand)Enum.Parse(typeof(EMyNetworkCommand), cmdId.ToString()))
        {
            case EMyNetworkCommand.COMMAND_AUTH:
            {
                MyAuthCommand authCmd = JsonUtility.FromJson<MyAuthCommand>(cmdJson);
                
                m_netClient.RegisterAuthToken(authCmd.AuthCode);
                m_netClient.RegisterClientID(authCmd.ClientID);
                break;
            }
        }
    }
}
