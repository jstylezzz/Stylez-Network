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
public class MyThreadedCreateObject : IMyCTOperation
{
    public int ObjectID { get; private set; }
    public Vector3 InstantiatePos { get; private set; }

    public MyThreadedCreateObject(int id, Vector3 pos)
    {
        this.ObjectID = id;
        this.InstantiatePos = pos;
    }
    
    public void Execute()
    {
        GameObject g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        MyNetworkObject nwo = g.AddComponent<MyNetworkObject>();
        nwo.UpdatePosition(InstantiatePos);
        nwo.SetupNetworkObject(this.ObjectID, 0);
    }
}
