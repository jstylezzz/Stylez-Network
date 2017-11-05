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
                if (cmd.WorldObjectLocations.Length > 0)
                {
                    MyThreadedCreateObject co;
                    for (int i = 0; i < cmd.WorldObjectLocations.Length; i++)
                    {
                        co = new MyThreadedCreateObject(cmd.WorldObjectIDs[i], new Vector3((float)cmd.WorldObjectLocations[i].x, (float)cmd.WorldObjectLocations[i].y, (float)cmd.WorldObjectLocations[i].z));
                        MyCrossThreadOperator.Instance.Enqueue(co);
                    }
                }
                break;
            }
            case EMyNetworkCommand.COMMAND_REQUEST_AREAUPDATE:
            {
                MyAreaUpdateCommand cmd = JsonUtility.FromJson<MyAreaUpdateCommand>(cmdJson);
                if (cmd.ObjectsNoLongerInRange == null) Debug.Log("No objects have streamed out.");
                else
                {
                    MyWorldObjectManager oman = MyWorldObjectManager.Instance;
                    int currentID;
                    MyNetworkObject currentObject;

                    for (int i = 0; i < cmd.ObjectsNoLongerInRange.Length; i++)
                    {
                        currentID = cmd.ObjectsNoLongerInRange[i];
                        Debug.Log("Object with ID " + currentID + " was streamed out.");
                        MyThreadedDeleteObject del = new MyThreadedDeleteObject(currentID, MyThreadedDeleteObject.EMyDeleteType.FROMREMOTE);
                        MyCrossThreadOperator.Instance.Enqueue(del);
                    }
                    
                    for (int i = 0; i < cmd.ObjectIDsInRange.Length; i++)
                    {
                        currentID = cmd.ObjectIDsInRange[i];

                        if (oman.NetObjectRegistry.TryGetValue(currentID, out currentObject)) //Object exists, update position
                        {
                            Vector3 newPos = new Vector3((float)cmd.ObjectPositions[i].x, (float)cmd.ObjectPositions[i].y, (float)cmd.ObjectPositions[i].z);
                            MyThreadedMoveObject mo = new MyThreadedMoveObject(currentObject, newPos);
                            MyCrossThreadOperator.Instance.Enqueue(mo);
                        }
                        else //Object does not exist, create it
                        {
                            MyThreadedCreateObject co = new MyThreadedCreateObject(currentID, new Vector3((float)cmd.ObjectPositions[i].x, (float)cmd.ObjectPositions[i].y, (float)cmd.ObjectPositions[i].z));
                            MyCrossThreadOperator.Instance.Enqueue(co);
                        }
                    }
                }
                break;
            }
        }
    }
}
