using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StylezNetworkDedicated.Network;
using StylezNetwork.Objects;

namespace StylezNetworkDedicated.Manager
{
    public class MyDedicatedClientData
    {
        public Dictionary<int, IMyNetworkObject> PlayerOwnedObjects { get { return m_registeredClientObjects; } }

        private MyDedicatedServerClient m_clientBase;
        private Dictionary<int, IMyNetworkObject> m_registeredClientObjects = new Dictionary<int, IMyNetworkObject>();
        private int[] m_lastObjectsInRange;

        public MyDedicatedClientData(MyDedicatedServerClient c)
        {
            m_clientBase = c;
        }

        public void RegisterPlayerWorldObject(IMyNetworkObject o)
        {
            m_registeredClientObjects.Add(o.ObjectNetworkID, o);
        }

        public void UnregisterPlayerWorldObject(IMyNetworkObject o)
        {
            m_registeredClientObjects.Remove(o.ObjectNetworkID);
        }

        /// <summary>
        /// Checks which objects are no longer in range, then updates the last in range cache.
        /// </summary>
        /// <param name="newObjects">The list of objects that are now in range.</param>
        /// <returns>Objects that are no longer in range</returns>
        public int[] UpdateObjectsInRange(int[] newObjects)
        {
            int[] noLonger = GetObjectsNoLongeInRange(newObjects);
            m_lastObjectsInRange = newObjects;
            return noLonger;
        }

        public int[] GetObjectsNoLongeInRange(int[] nowInRange)
        {
            HashSet<int> now = new HashSet<int>(nowInRange);
            HashSet<int> noLonger = new HashSet<int>();
            if (m_lastObjectsInRange != null)
            {
                for (int i = 0; i < m_lastObjectsInRange.Length; i++)
                {
                    if (!now.Contains(m_lastObjectsInRange[i])) noLonger.Add(m_lastObjectsInRange[i]);
                }
                return noLonger.ToArray();
            }
            return null;
        }
    }
}
