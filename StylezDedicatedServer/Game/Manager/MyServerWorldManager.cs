using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StylezNetworkShared.Game.World.Objects;

namespace StylezDedicatedServer.Game.Manager
{
    public delegate void OnObjectRegisteredDelegate(int clientIDOwner, int objectID);
    public delegate void OnObjectUnregisteredDelegate(int clientIDOwner, int objectID);

    /// <summary>
    /// This class takes care of a global registry (collection) of objects.
    /// </summary>
    public class MyServerWorldManager
    {
        public static MyServerWorldManager Instance { get; private set; }
        /// <summary>
        /// Event called when a new object is registered.
        /// </summary>
        public event OnObjectRegisteredDelegate OnObjectRegistered;

        /// <summary>
        /// Event called when an object is unregistered.
        /// </summary>
        public event OnObjectUnregisteredDelegate OnObjectUnregistered;

        private Dictionary<int, MyWorldObject> m_worldObjectRegistry = new Dictionary<int, MyWorldObject>();
        private Stack<int> m_freeIds = new Stack<int>();
        private int m_highestID = -1;


        public MyServerWorldManager()
        {
            if (Instance == null) Instance = this;
            else return;
        }

        /// <summary>
        /// Register an object to the world manager.
        /// </summary>
        /// <param name="o">The WorldObject instance to register.</param>
        public void RegisterObject(MyWorldObject o)
        {
            o.ObjectID = GetFreeObjectID();
            m_worldObjectRegistry.Add(o.ObjectID, o);
            OnObjectRegistered?.Invoke(o.OwnerID, o.ObjectID);
        }

        /// <summary>
        /// Unregister an object from the registry.
        /// </summary>
        /// <param name="o">The WorldObject to unregister.</param>
        public void UnregisterObject(MyWorldObject o)
        {
            ReturnObjectIDToPool(o.ObjectID);
            OnObjectUnregistered?.Invoke(o.OwnerID, o.ObjectID);
            m_worldObjectRegistry.Remove(o.ObjectID);
        }

        /// <summary>
        /// Place an object ID in the pool of available ID's.
        /// </summary>
        /// <param name="id">The ID to place in the pool.</param>
        private void ReturnObjectIDToPool(int id)
        {
            m_freeIds.Push(id);
        }

        /// <summary>
        /// Get the owner ID for a specific object.
        /// </summary>
        /// <param name="objectID">The object ID to get the owner from.</param>
        /// <returns>The owner ID for the specified object.</returns>
        public int GetObjectOwner(int objectID)
        {
            return m_worldObjectRegistry[objectID].OwnerID;
        }

        /// <summary>
        /// Get a free object ID. If the ID pool has a free ID that can be re-used, that one
        /// will be used. No free ID's? Then a new object ID will be picked (highest existing + 1).
        /// </summary>
        /// <param name="markInUse">True to mark this ID in use (remove it from the available possibilities), false to let the ID be available.</param>
        /// <returns></returns>
        private int GetFreeObjectID(bool markInUse = true)
        {
            if (m_freeIds.Count == 0)
            {
                if (markInUse) m_highestID++;
                return m_highestID;
            }
            else
            {
                if (markInUse) return m_freeIds.Pop();
                else return m_freeIds.Peek();
            }
        }

        /// <summary>
        /// Get an array of WorldObjects by their ids.
        /// </summary>
        /// <param name="objectids">An array with all the ID's of the WorldObject instances you want.</param>
        /// <returns>An array of WorldObject instances.</returns>
        public MyWorldObject[] GetObjects(int[] objectids)
        {
            if (objectids == null) return null;
            MyWorldObject[] all = new MyWorldObject[objectids.Length];
            for (int i = 0; i < objectids.Length; i++) all[i] = m_worldObjectRegistry[objectids[i]];
            return all;
        }

        /// <summary>
        /// Get a WorldObject instance by its ID.
        /// </summary>
        /// <param name="id">The ID of the WorldObject instance to get.</param>
        /// <returns>The WorldObject instance for the specified ID.</returns>
        public MyWorldObject GetObject(int id)
        {
            return m_worldObjectRegistry[id];
        }
    }
}
