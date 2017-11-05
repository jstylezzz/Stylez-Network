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
public class MyThreadedMoveObject : IMyCTOperation
{
    public enum EMyThreadedMoveUpdate
    {
        SET_POS,
        UPDATE_MOV
    }

    public Vector3 MovePosition { get; private set; }
    public MyNetworkObject TheObject { get; private set; }
    public EMyThreadedMoveUpdate UType{ get; private set; }
    public MyObjectMovementData MoveData { get; private set; }

    public MyThreadedMoveObject(MyNetworkObject o, Vector3 p, EMyThreadedMoveUpdate up)
    {
        this.TheObject = o;
        this.MovePosition = p;
        UType = up;
        MoveData = null;
    }

    public MyThreadedMoveObject(MyNetworkObject o, MyObjectMovementData md, EMyThreadedMoveUpdate up)
    {
        TheObject = o;
        MoveData = md;
        UType = up;
    }

    public void Execute()
    {
        if (UType == EMyThreadedMoveUpdate.SET_POS) this.TheObject.RemoteUpdatePosition(this.MovePosition);
        else if (UType == EMyThreadedMoveUpdate.UPDATE_MOV)
        {
            if (MoveData.MovementState != EMyObjectMovementState.MOVEMENT_NONE) TheObject.UpdateMovementValues(MoveData);
        }
    }
}
