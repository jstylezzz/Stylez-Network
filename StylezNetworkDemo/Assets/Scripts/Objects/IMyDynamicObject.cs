/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StylezNetworkShared.Objects;

/// <summary>
///
/// </summary>
public interface IMyDynamicObject 
{
    Rigidbody DynamicObjectBody { get; }
    MyDynamicObject DynamicObjectInstance { get; }
}
