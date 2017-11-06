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
public class MyPlayerController : MonoBehaviour 
{
    [SerializeField]
    private double m_speed;

    private MyNetworkObject m_netObject;
    private Vector2 m_move = Vector2.zero;
   

	/// <summary>
	/// Script entry point.
	/// </summary>
	private void Start () 
	{
        Camera.main.transform.parent = transform;
        Camera.main.transform.localPosition = new Vector3(0, 0, -10);
	}
	
	/// <summary>
	/// Script update look.
	/// </summary>
	private void Update () 
	{
        if (m_netObject == null) m_netObject = GetComponent<MyNetworkObject>();
        if(m_netObject != null)
        {
            if (m_netObject.UnderLocalControl == false) m_netObject.UnderLocalControl = true;
            Vector2 beforeProcess = m_move;

            if (Input.GetKeyDown(KeyCode.W)) m_move += Vector2.up;
            else if (Input.GetKeyUp(KeyCode.W)) m_move -= Vector2.up;

            if (Input.GetKeyDown(KeyCode.A)) m_move += Vector2.left;
            else if (Input.GetKeyUp(KeyCode.A)) m_move -= Vector2.left;

            if (Input.GetKeyDown(KeyCode.D)) m_move += Vector2.right;
            else if (Input.GetKeyUp(KeyCode.D)) m_move -= Vector2.right;

            if (Input.GetKeyDown(KeyCode.S)) m_move += Vector2.down;
            else if (Input.GetKeyUp(KeyCode.S)) m_move -= Vector2.down;

            //If anything changed
            if(beforeProcess != m_move)
            {
                if (m_move == Vector2.zero) m_netObject.StopMove();
                else m_netObject.StartMove(m_move, m_speed);
                m_netObject.UpdateDirection(m_move);
            }
        }
	}
}
