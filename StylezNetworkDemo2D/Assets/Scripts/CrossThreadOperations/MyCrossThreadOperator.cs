/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class MyCrossThreadOperator : MonoBehaviour 
{
    public static MyCrossThreadOperator Instance { get; private set; }

    public static readonly Queue<Action> ActionQueue = new Queue<Action>();
    public static readonly Queue<IMyCTOperation> CrossThreadOps = new Queue<IMyCTOperation>();

	/// <summary>
	/// Script entry point.
	/// </summary>
	private void Start () 
	{
        Instance = this;
	}
	
	/// <summary>
	/// Script update look.
	/// </summary>
	private void Update () 
	{
		lock(ActionQueue)
        {
            while (ActionQueue.Count > 0) ActionQueue.Dequeue()?.Invoke();
        }

        lock(CrossThreadOps)
        {
            while (CrossThreadOps.Count > 0) CrossThreadOps.Dequeue()?.Execute();
        }
	}

    public void Enqueue(Action a)
    {
        ActionQueue.Enqueue(a);
    }

    public void Enqueue(IMyCTOperation c)
    {
        CrossThreadOps.Enqueue(c);
    }
}
