using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StylezNetwork.Objects;
using StylezNetwork.MathEx;

namespace StylezNetworkDedicated.Manager
{
    public class MyServerWorldCache
    {
        private Dictionary<int, IMyNetworkObject> m_worldObjects = new Dictionary<int, IMyNetworkObject>();
        private Stack<int> m_freeIndexStack = new Stack<int>();
        private int m_highestObjectID = -1;

        public void AddObjectToWorld(IMyNetworkObject o)
        {
            m_worldObjects.Add(o.ObjectNetworkID, o);
            Program.Instance.ServerBaseInstance.ClientRegistry[o.OwnerClientID].RegisterPlayerWorldObject(o);
            Console.WriteLine($"[DBG]: Object added to world with ID {o.ObjectNetworkID} from client {o.OwnerClientID}.");
        }

        public void RemoveObjectFromWorld(IMyNetworkObject o)
        {
            int id = o.ObjectNetworkID;
            m_freeIndexStack.Push(id);
            m_worldObjects.Remove(id);
            Program.Instance.ServerBaseInstance.ClientRegistry[o.OwnerClientID].UnregisterPlayerWorldObject(o);
            Console.WriteLine($"[DBG]: Remove object with ID {id} with owner id {o.OwnerClientID} from the world.");
        }

        public IMyNetworkObject GetNetworkObject(int id)
        {
            return m_worldObjects[id];
        }

        public IMyNetworkObject[] GetNetworkObjects(Vector3Simple point, int dim, double dist)
        {
            List<IMyNetworkObject> objects = new List<IMyNetworkObject>();
            foreach(IMyNetworkObject o in m_worldObjects.Values)
            {
                if (o.Dimension == dim && Vector3Simple.Distance(o.Position, point) <= dist) objects.Add(o);
            }
            return objects.ToArray();
        }

        public void RemoveAllObjectsFromPlayer(IMyNetworkObject[] o, int clientid)
        {
            for (int i = 0; i < o.Length; i++)
            {
                //Check if the client is the owner, just to be sure
                if (o[i].OwnerClientID == clientid) RemoveObjectFromWorld(o[i]);
            }
        }

        public int GetFirstFreeObjectID(bool removeUsed = false)
        {
            if (removeUsed)
            {
                if (m_freeIndexStack.Count == 0)
                {
                    m_highestObjectID++;
                    return m_highestObjectID;
                }
                else return m_freeIndexStack.Pop();
            }
            else
            {
                if (m_freeIndexStack.Count == 0) return m_highestObjectID + 1;
                else return m_freeIndexStack.Peek();
            }
        }
    }
}
