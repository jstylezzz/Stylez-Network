/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StylezNetwork.Objects;

/// <summary>
///
/// </summary>
public class MyThreadedCreateObject : IMyCTOperation
{
    public MySimpleWorldObject ObjectData { get; private set; }

    public MyThreadedCreateObject(MySimpleWorldObject o)
    {
        ObjectData = o;
    }
    
    public void Execute()
    {
        GameObject g;

        if (ObjectData.ObjectType == EMyObjectType.OBJECT_PLAYER)
        {
            //It's our player
            if (ObjectData.OwnerClientID == MyDemoNetworkClient.Instance.ClientID) g = GameObject.Instantiate(Resources.Load<GameObject>(@"Prefabs\Player_Local"));
            else g = GameObject.Instantiate(Resources.Load<GameObject>(@"Prefabs\Player_Remote"));
        }
        else g = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        MyNetworkObject nwo = g.AddComponent<MyNetworkObject>();
        nwo.RemoteUpdatePosition(new Vector3((float)ObjectData.Position.x, (float)ObjectData.Position.y, (float)ObjectData.Position.z));
        nwo.SetupNetworkObject(ObjectData.ObjectNetworkID, 0);
    }
}
