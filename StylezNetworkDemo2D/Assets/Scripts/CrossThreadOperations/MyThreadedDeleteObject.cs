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
public class MyThreadedDeleteObject : IMyCTOperation
{
    public enum EMyDeleteType
    {
        /// <summary>
        /// Object delete requested by server
        /// </summary>
        FROMREMOTE,

        /// <summary>
        /// Object delete requested locally
        /// </summary>
        FROMLOCAL
    }

    public int TheObject { get; private set; }
    public EMyDeleteType DelType { get; private set; }

    public MyThreadedDeleteObject(int o, EMyDeleteType t)
    {
        this.TheObject = o;
        this.DelType = t;
    }

    public void Execute()
    {
        if(DelType == EMyDeleteType.FROMLOCAL) MyWorldObjectManager.FromLocalDeleteObject(MyWorldObjectManager.Instance.NetObjectRegistry[this.TheObject]);
        else if (DelType == EMyDeleteType.FROMREMOTE) MyWorldObjectManager.FromRemoteDeleteObject(MyWorldObjectManager.Instance.NetObjectRegistry[this.TheObject]);

    }
}
