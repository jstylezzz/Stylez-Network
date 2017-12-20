﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StylezDedicatedServer.Game.Manager
{
    public class MyClientEntityCollection
    {
        public int[] RangedObjects { get; private set; }
        public int[] OwnedObjects { get; private set; }

        private List<int> m_ownedWorldObjectIds = new List<int>();
        private List<int> m_objectsInRange = new List<int>();

        public void AddOwnedObject(int id)
        { 
            m_ownedWorldObjectIds.Add(id);
            OwnedObjects = m_ownedWorldObjectIds.ToArray();
            AddRangedObject(id); //Temporary, until ranged system is in place
        }

        public void RemoveOwnedObject(int id)
        {
            m_ownedWorldObjectIds.Remove(id);
            OwnedObjects = m_ownedWorldObjectIds.ToArray();
            RemoveRangedObject(id); //Temporary, until ranged system is in place
        }

        public void AddRangedObject(int id)
        {
            m_objectsInRange.Add(id);
            RangedObjects = m_objectsInRange.ToArray();
        }

        public void RemoveRangedObject(int id)
        {
            m_objectsInRange.Remove(id);
            RangedObjects = m_objectsInRange.ToArray();
        }

    }
}
