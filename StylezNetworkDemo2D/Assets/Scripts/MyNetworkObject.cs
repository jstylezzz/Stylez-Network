/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StylezNetwork.Commands;
using StylezNetwork.MathEx;
using StylezNetwork.Objects;

/// <summary>
///
/// </summary>
public class MyNetworkObject : MonoBehaviour
{
    public int ObjectID { get; private set; }
    public Vector3 Position { get { return transform.position; } }
    public int Dimension { get; private set; }
    public double Speed{ get; private set; }
    public Vector3 MoveDirection { get; private set; }
    public bool MoveObject { get; private set; } = false;

    private MyWorldObjectManager m_worldMan;

	/// <summary>
	/// Script entry point.
	/// </summary>
	private void Start () 
	{
        
    }

    public void SetupNetworkObject(int id, int dimension)
    {
        m_worldMan = MyWorldObjectManager.Instance;
        this.ObjectID = id;
        this.Dimension = dimension;
        m_worldMan.RegisterObject(this);
    }
	
    /// <summary>
    /// Update permission as requested from the server.
    /// No message is sent back to the server or other clients.
    /// </summary>
    /// <param name="newPosition">New location.</param>
    public void RemoteUpdatePosition(Vector3 newPosition)
    {
        transform.position = newPosition;
        Debug.Log("Server moved me" + gameObject.name + " at " + transform.position);
    }

    /// <summary>
    /// Update permission as requested from the server.
    /// A message is sent back to the server with updated values.
    /// </summary>
    /// <param name="newPosition">New location.</param>
    public void LocalUpdatePosition(Vector3 newPosition)
    {
        transform.position = newPosition;

        MyMoveObjectCommand cmd = new MyMoveObjectCommand(new MyObjectMovementData(new Vector3Simple(newPosition.x, newPosition.y, newPosition.z)), ObjectID);
        MyDemoNetworkClient.Instance.EnqueueMessage(JsonUtility.ToJson(cmd), (int)EMyNetworkCommand.COMMAND_MOVE_OBJECT);
    }

    public void StartMove(Vector3 direction, double speed)
    {
        MyObjectMovementData md = new MyObjectMovementData(new Vector3Simple(transform.position.x, transform.position.y, transform.position.z), new Vector3Simple(direction.x, direction.y, direction.z), speed, EMyObjectMovementState.MOVEMENT_START);
        MyMoveObjectCommand cmd = new MyMoveObjectCommand(md, ObjectID);
        MyDemoNetworkClient.Instance.EnqueueMessage(JsonUtility.ToJson(cmd), (int)EMyNetworkCommand.COMMAND_MOVE_OBJECT);
        MoveObject = true;
        Debug.Log("Started moving " + gameObject.name + " at " + transform.position);
    }

    public void StopMove()
    {
        MyObjectMovementData md = new MyObjectMovementData(new Vector3Simple(transform.position.x, transform.position.y, transform.position.z), EMyObjectMovementState.MOVEMENT_STOP);
        MyMoveObjectCommand cmd = new MyMoveObjectCommand(md, ObjectID);
        MyDemoNetworkClient.Instance.EnqueueMessage(JsonUtility.ToJson(cmd), (int)EMyNetworkCommand.COMMAND_MOVE_OBJECT);
        MoveObject = false;
        Debug.Log("Stopping moving " + gameObject.name + " at " + transform.position);
    }

    public void UpdateMovementValues(MyObjectMovementData md)
    {
        
        if (md.MovementState == EMyObjectMovementState.MOVEMENT_STOP && MoveObject)
        {
            MoveObject = false;
            transform.position = new Vector3((float)md.CurrentLocation.x, (float)md.CurrentLocation.y, (float)md.CurrentLocation.z);
            Debug.Log("Updating " + gameObject.name + " to " + md.CurrentLocation);
        }
        else if (md.MovementState == EMyObjectMovementState.MOVEMENT_START)
        {
            MoveObject = true;
            Speed = md.MovementSpeed;
            MoveDirection = new Vector3((float)md.MovementDirection.x, (float)md.MovementDirection.y, (float)md.MovementDirection.z);
        }
        else if (md.MovementState == EMyObjectMovementState.MOVEMENT_POSITION_UPDATE) transform.position = new Vector3((float)md.CurrentLocation.x, (float)md.CurrentLocation.y, (float)md.CurrentLocation.z);
    }

	/// <summary>
	/// Script update look.
	/// </summary>
	private void Update () 
	{
		if(MoveObject)
        {
            transform.Translate((MoveDirection * (float)Speed) * Time.deltaTime);
        }
	}

    private void OnDestroy()
    {
        m_worldMan.UnregisterWorldObject(this);   
    }
}
