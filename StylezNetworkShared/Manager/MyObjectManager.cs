using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StylezNetworkShared.Objects;

namespace StylezNetworkShared.Manager
{
    public class MyObjectManager
    {
        private static Dictionary<int, MyDynamicObject> DynamicObjectRegistry = new Dictionary<int, MyDynamicObject>();
        
        public delegate void OnObjectAddedToRegistryDelegate(int ownerid, int objectid);
        public delegate void OnObjectRemovedFromRegistryDelegate(int ownerid, int objectid);

        public event OnObjectAddedToRegistryDelegate OnObjectAddedToRegistry;
        public event OnObjectRemovedFromRegistryDelegate OnObjectRemovedFromRegistry;

        private Stack<int> m_freeIds = new Stack<int>();
        private int m_highestID = -1;

        public void RegisterDynamicObject(MyDynamicObject dyno)
        {
            if (!DynamicObjectRegistry.ContainsKey(dyno.ObjectID))
            {
                DynamicObjectRegistry.Add(dyno.ObjectID, dyno);
                OnObjectAddedToRegistry?.Invoke(dyno.OwnerClientID, dyno.ObjectID);
            }
        }

        public void RegisterNewDynamicObject(MyDynamicObject dyno)
        {
            dyno.SetObjectID(GetFreeObjectID());
            RegisterDynamicObject(dyno);
        }

        public void UnregisterDynamicObject(int id)
        {
            if(DynamicObjectRegistry.ContainsKey(id))
            {
                OnObjectRemovedFromRegistry?.Invoke(DynamicObjectRegistry[id].OwnerClientID, id);
                DynamicObjectRegistry.Remove(id);
                ReturnObjectIDToPool(id);
            }
        }

        public void UpdateDynamicObject(MyDynamicObject dyno)
        {
            if(DynamicObjectRegistry.ContainsKey(dyno.ObjectID))
            {
                DynamicObjectRegistry[dyno.ObjectID] = dyno;
            }
        }

        public MyDynamicObject GetDynamicObject(int id)
        {
            if (DynamicObjectRegistry.ContainsKey(id)) return DynamicObjectRegistry[id];
            else return null;
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

        public MyDynamicObject[] GetObjects(int[] ids)
        {
            MyDynamicObject[] found = new MyDynamicObject[ids.Length];
            int cIdx = 0;
            foreach (int idx in ids)
            {
                if(DynamicObjectRegistry.ContainsKey(idx))
                {
                    found[cIdx] = DynamicObjectRegistry[idx];
                    cIdx++;
                }
            }
            return found;
        }

        public MyDynamicObject[] GetAllObjects()
        {
            return DynamicObjectRegistry.Values.ToArray();
        }

        public int GetObjectOwner(int id)
        {
            if (DynamicObjectRegistry.ContainsKey(id)) return DynamicObjectRegistry[id].OwnerClientID;
            else return -1;
        }
    }
}
