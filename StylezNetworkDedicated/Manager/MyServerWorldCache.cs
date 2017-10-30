using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StylezNetwork.Objects;

namespace StylezNetworkDedicated.Manager
{
    public class MyServerWorldCache
    {
        private Dictionary<int, IMyNetworkObject> m_worldObjects = new Dictionary<int, StylezNetwork.Objects.IMyNetworkObject>();
        private Stack<int> m_freeIndexStack = new Stack<int>();
        private int m_highestObjectID = -1;

        public void AddObjectToWorld(IMyNetworkObject o)
        {
            m_worldObjects.Add(GetFirstFreeObjectID(true), o);
        }

        public void RemoveObjectFromWorld(IMyNetworkObject o)
        {
            int id = o.ObjectNetworkID;
            m_freeIndexStack.Push(id);
            m_worldObjects.Remove(id);
        }

        public IMyNetworkObject GetNetworkObject(int id)
        {
            return m_worldObjects[id];
        }

        private int GetFirstFreeObjectID(bool removeUsed = false)
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
