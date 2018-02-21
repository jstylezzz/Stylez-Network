using Newtonsoft.Json;
using StylezDedicatedServer.Core;
using StylezDedicatedServer.Threading;
using StylezNetworkShared.Game.Commands;
using StylezNetworkShared.Logging;
using StylezNetworkShared.Objects;
using StylezNetworkShared.Commands;
using System.Collections.Generic;
using System.Threading;

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
        private static Queue<MyAreaUpdateRequest> m_threadedUpdateRequests = new Queue<MyAreaUpdateRequest>();

        public MyClientWorldManager()
        {
            if (Instance == null) Instance = this;
            else return;

            MyDedicatedServer.ServerObjectManager.OnObjectAddedToRegistry += OnObjectRegistered;
            MyDedicatedServer.ServerObjectManager.OnObjectRemovedFromRegistry += OnObjectUnregistered;
            MyClientManager.Instance.OnClientConnected += OnClientConnected;
            MyClientManager.Instance.OnClientDisconnected += OnClientDisconnected;

            new Thread(ThreadedAreaUpdate).Start();
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
            foreach(MyDynamicObject obj in MyDedicatedServer.ServerObjectManager.GetObjects(o))
            { 
                if (obj.DestroyOnOwnerDisconnect == true) MyDedicatedServer.ServerObjectManager.UnregisterDynamicObject(obj.ObjectID);
            }
            m_clientEntityCollection.Remove(clientID);
        }

        private void OnObjectRegistered(int clientID, int objectID)
        {
            //If this object is owned by the given client ID, add it to their owned object collection.
            if (MyDedicatedServer.ServerObjectManager.GetObjectOwner(objectID) == clientID) m_clientEntityCollection[clientID].AddOwnedObject(objectID);
        }

        private void OnObjectUnregistered(int clientID, int objectID)
        {
            //If this object is owned by the given client ID, remove it from their owned object collection.
            if (MyDedicatedServer.ServerObjectManager.GetObjectOwner(objectID) == clientID) m_clientEntityCollection[clientID].RemoveOwnedObject(objectID);
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

        /// <summary>
        /// Perform an area update for a client.
        /// On completion, new area data is sent to the client.
        /// </summary>
        /// <param name="ar">Area update request instance.</param>
        public void PerformClientAreaUpdate(MyAreaUpdateRequest ar)
        {
            m_threadedUpdateRequests.Enqueue(ar);
        }

        private void ThreadedAreaUpdate()
        {
            MyThreadManager tm = MyThreadManager.Instance;
            MyAreaUpdateRequest ar;
            while (tm.ThreadingRunning)
            {
                if(m_threadedUpdateRequests.Count > 0)
                {
                    ar = m_threadedUpdateRequests.Dequeue();
                    List<int> inRange = new List<int>();
                    List<MyDynamicObject> inRangeO = new List<MyDynamicObject>();
                    MyDynamicObject[] wos = MyDedicatedServer.ServerObjectManager.GetAllObjects();

                    foreach (MyDynamicObject w in wos)
                    {
                        if (w.ObjectDimension == ar.Dimension && w.DistanceTo(ar.PosX, ar.PosY, ar.PosZ) <= ar.StreamDistance)
                        {
                            inRange.Add(w.ObjectID);
                            inRangeO.Add(w);
                        }
                    }
                    MyClientEntityCollection ec = GetCollectionForClient(ar.ForClientID);
                    ec?.UpdateRangedObjectList(inRange.ToArray());
                    MyClientManager.Instance.SendTransmissionToClient(ar.ForClientID, new StylezNetworkShared.Commands.MyNetCommand((int)EMyNetworkCommands.WORLD_AREA_UPDATE, JsonConvert.SerializeObject(new MyDynamicObjectAreaUpdate(ec.RangedObjects.Length, inRangeO.ToArray()))));
                }
            }
            MyLogger.LogInfo("Threaded area update stopping..");
        }
    }
}
