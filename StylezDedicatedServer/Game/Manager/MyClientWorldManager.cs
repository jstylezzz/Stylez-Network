using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StylezDedicatedServer.Core;
using StylezNetworkShared.Game.World.Objects;

namespace StylezDedicatedServer.Game.Manager
{
    /// <summary>
    /// This class takes care of client world data serverside.
    /// Routines like checking objects in range, keeping track
    /// of one's objects are done here.
    /// </summary>
    public class MyClientWorldManager
    {
        public static MyClientWorldManager Instance { get; private set; }

        private Dictionary<int, MyClientEntityCollection> m_clientEntityCollection = new Dictionary<int, MyClientEntityCollection>();

        public MyClientWorldManager()
        {
            if (Instance == null) Instance = this;
            else return;

            MyServerWorldManager.Instance.OnObjectRegistered += OnObjectRegistered;
            MyServerWorldManager.Instance.OnObjectUnregistered += OnObjectUnregistered;
            MyClientManager.Instance.OnClientConnected += OnClientConnected;
            MyClientManager.Instance.OnClientDisconnected += OnClientDisconnected;
        }

        private void OnClientConnected(int clientID)
        {
            //Add a new collection for this client
            m_clientEntityCollection.Add(clientID, new MyClientEntityCollection());
        }

        private void OnClientDisconnected(int clientID)
        {
            //Remove the collection for this client
            MyClientEntityCollection ec = m_clientEntityCollection[clientID];
            int[] o = ec.OwnedObjects;
            foreach(MyWorldObject obj in MyServerWorldManager.Instance.GetObjects(o))
            {
                if (obj.DestroyOnDisconnect == true) MyServerWorldManager.Instance.UnregisterObject(obj);
            }
            m_clientEntityCollection.Remove(clientID);
        }

        private void OnObjectRegistered(int clientID, int objectID)
        {
            //If this object is owned by the given client ID, add it to their owned object collection.
            if (MyServerWorldManager.Instance.GetObjectOwner(objectID) == clientID) m_clientEntityCollection[clientID].AddOwnedObject(objectID);
        }

        private void OnObjectUnregistered(int clientID, int objectID)
        {
            //If this object is owned by the given client ID, remove it from their owned object collection.
            if (MyServerWorldManager.Instance.GetObjectOwner(objectID) == clientID) m_clientEntityCollection[clientID].RemoveOwnedObject(objectID);
        }

        /// <summary>
        /// Get the ClientEntityCollection for a client.
        /// </summary>
        /// <param name="clientid">The clientID to get the collection for.</param>
        /// <returns>A ClientEntityCollection for the specified client.</returns>
        public MyClientEntityCollection GetCollectionForClient(int clientid)
        {
            return m_clientEntityCollection[clientid];
        }
    }
}
