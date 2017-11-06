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
    public bool IsMoving { get; private set; } = false;
    public Vector3 MovementStartedAt { get; private set; }
    public Vector3 MovementStoppedAt { get; private set; }
    public bool HasChanged { get; set; } = false;
    public Vector3 LastNetworkLocation { get; private set; }
    public bool UnderLocalControl { get; set; } = false;
    public bool StartSent { get; private set; } = false;
    public bool StopSent { get; private set; } = false;
   

    private MyWorldObjectManager m_worldMan;

	/// <summary>
	/// Script entry point.
	/// </summary>
	private void Start () 
	{
        
    }

    public void UpdateDirection(Vector3 newdir)
    {
        MoveDirection = newdir;
        HasChanged = true;
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
        MoveDirection = direction;
        Speed = speed;
        MoveObject = true;
        StartSent = false;
    }

    public void StopMove()
    {
        MoveDirection = Vector3.zero;
        Speed = 0d;
        MoveObject = false;
        StopSent = false;
    }

    public void UpdateMovementValues(MyObjectMovementData md)
    {
        if (UnderLocalControl) return;
        Debug.Log(md.MovementState);
        if (md.MovementState == EMyObjectMovementState.MOVEMENT_STOP && MoveObject && IsMoving)
        {
            MoveObject = false;
            MovementStoppedAt = new Vector3((float)md.CurrentLocation.x, (float)md.CurrentLocation.y, (float)md.CurrentLocation.z);
        }
        else if (md.MovementState == EMyObjectMovementState.MOVEMENT_START && !MoveObject && !IsMoving)
        {
            MoveObject = true;
            Speed = md.MovementSpeed;
            MovementStartedAt = new Vector3((float)md.StartStopLocation.x, (float)md.StartStopLocation.y, (float)md.StartStopLocation.z);
            MoveDirection = new Vector3((float)md.MovementDirection.x, (float)md.MovementDirection.y, (float)md.MovementDirection.z);
        }
        else if (md.MovementState == EMyObjectMovementState.MOVEMENT_UPDATE && IsMoving && MoveObject)
        {
            MoveDirection = new Vector3((float)md.MovementDirection.x, (float)md.MovementDirection.y, (float)md.MovementDirection.z);
            LastNetworkLocation = new Vector3((float)md.CurrentLocation.x, (float)md.CurrentLocation.y, (float)md.CurrentLocation.z);
            Speed = md.MovementSpeed;
        }
        else if (md.MovementState == EMyObjectMovementState.MOVEMENT_UPDATE && !IsMoving && !MoveObject)
        {
            StartMove(new Vector3((float)md.CurrentLocation.x, (float)md.CurrentLocation.y, (float)md.CurrentLocation.z), md.MovementSpeed);
        }
        else if (md.MovementState == EMyObjectMovementState.MOVEMENT_POSITION_UPDATE) transform.position = new Vector3((float)md.CurrentLocation.x, (float)md.CurrentLocation.y, (float)md.CurrentLocation.z);
    }

    private void SendStartMoveCommand()
    {
        HasChanged = false;
        StartSent = true;
        MyObjectMovementData md = new MyObjectMovementData(new Vector3Simple(transform.position.x, transform.position.y, transform.position.z), new Vector3Simple(MoveDirection.x, MoveDirection.y, MoveDirection.z), Speed, EMyObjectMovementState.MOVEMENT_START, new Vector3Simple(MovementStartedAt.x, MovementStartedAt.y, MovementStartedAt.z));
        MyMoveObjectCommand cmd = new MyMoveObjectCommand(md, ObjectID);
        MyDemoNetworkClient.Instance.EnqueueMessage(JsonUtility.ToJson(cmd), (int)EMyNetworkCommand.COMMAND_MOVE_OBJECT);
    }

    private void SendStopMoveCommand()
    {
        HasChanged = false;
        StopSent = true;
        MyObjectMovementData md = new MyObjectMovementData(new Vector3Simple(transform.position.x, transform.position.y, transform.position.z), new Vector3Simple(MoveDirection.x, MoveDirection.y, MoveDirection.z), Speed, EMyObjectMovementState.MOVEMENT_STOP, new Vector3Simple(MovementStoppedAt.x, MovementStoppedAt.y, MovementStoppedAt.z));
        MyMoveObjectCommand cmd = new MyMoveObjectCommand(md, ObjectID);
        MyDemoNetworkClient.Instance.EnqueueMessage(JsonUtility.ToJson(cmd), (int)EMyNetworkCommand.COMMAND_MOVE_OBJECT);
    }

    /// <summary>
    /// Script update look.
    /// </summary>
    private void Update () 
	{
        if (MoveObject)
        {
            if (!IsMoving)
            {
                IsMoving = true;
                HasChanged = true;
                if (UnderLocalControl) MovementStartedAt = transform.position;
                //else transform.position = MovementStoppedAt;
                if (!StartSent && UnderLocalControl) SendStartMoveCommand();
            }
            transform.Translate((MoveDirection * (float)Speed) * Time.deltaTime);
            if (!UnderLocalControl)
            {
                //if (Vector3.Distance(transform.position, LastNetworkLocation) > 0.5f) transform.position = LastNetworkLocation;
            }
        }
        else if (!MoveObject && IsMoving)
        {
            IsMoving = false;
            if (UnderLocalControl) MovementStoppedAt = transform.position;
            else transform.position = MovementStoppedAt;
            HasChanged = true;
            if (!StopSent && UnderLocalControl) SendStopMoveCommand();
        }
	}

    private void OnDestroy()
    {
        m_worldMan.UnregisterWorldObject(this);   
    }
}
