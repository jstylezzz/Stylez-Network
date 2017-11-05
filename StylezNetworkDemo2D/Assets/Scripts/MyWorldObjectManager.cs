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

/// <summary>
///
/// </summary>
public class MyWorldObjectManager : MonoBehaviour 
{
    public static MyWorldObjectManager Instance { get; private set; }

    private Dictionary<int, MyNetworkObject> m_objectRegistry = new Dictionary<int, MyNetworkObject>();
    public Dictionary<int, MyNetworkObject> NetObjectRegistry { get { return m_objectRegistry; } }

	/// <summary>
	/// Script entry point.
	/// </summary>
	private void Start () 
	{
        Instance = this;
        MyRequestAllObjectsCommand cmdc = new MyRequestAllObjectsCommand(new Vector3Simple(transform.position.x, transform.position.y, transform.position.z), 0, 50);
        MyDemoNetworkClient.Instance.EnqueueMessage(JsonUtility.ToJson(cmdc), (int)EMyNetworkCommand.COMMAND_WORLD_GETOBJECTS);
    }
	
	public void RegisterObject(MyNetworkObject o)
    {
        m_objectRegistry.Add(o.ObjectID, o);
    }

    public void UnregisterWorldObject(MyNetworkObject o)
    {
        m_objectRegistry.Remove(o.ObjectID);
    }

    public static void CreateObject(Vector3 position, int dimension)
    {
        MyCreateObjectCommand cmd = new MyCreateObjectCommand(new Vector3Simple(position.x, position.y, position.z), dimension);
        MyDemoNetworkClient.Instance.EnqueueMessage(JsonUtility.ToJson(cmd), (int)EMyNetworkCommand.COMMAND_OBJECT_CREATE);
    }

    /// <summary>
    /// Delete object as instructed by local game. This means we send a
    /// command to the server to delete it everywhere.
    /// </summary>
    /// <param name="g">The gameobject to delete.</param>
    public static void FromLocalDeleteObject(MyNetworkObject o)
    {
        MyDeleteObjectCommand cmd = new MyDeleteObjectCommand(o.ObjectID);
        MyDemoNetworkClient.Instance.EnqueueMessage(JsonUtility.ToJson(cmd), (int)EMyNetworkCommand.COMMAND_OBJECT_DELETE);
        Destroy(o.gameObject);
    }

    /// <summary>
    /// Delete object as instructed by the server. This means we do not
    /// send a command to the server to delete it everywhere.
    /// </summary>
    /// <param name="g">The gamemobject to delete.</param>
    public static void FromRemoteDeleteObject(MyNetworkObject g)
    {
        Destroy(g.gameObject);
    }
}
