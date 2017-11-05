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
public class MyThreadedMoveObject : IMyCTOperation
{
    public Vector3 MovePosition { get; private set; }
    public MyNetworkObject TheObject { get; private set; }

    public MyThreadedMoveObject(MyNetworkObject o, Vector3 p)
    {
        this.TheObject = o;
        this.MovePosition = p;
    }

    public void Execute()
    {
        this.TheObject.UpdatePosition(this.MovePosition);
    }
}
