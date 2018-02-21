using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StylezNetworkShared.Game.World.Objects
{
    public class MyObjectBase
    {
        public string PrefabName { get; private set; }
        public bool DestroyOnOwnerDisconnect { get; private set; }
        
        public int OwnerClientID { get; private set; }
        public int ObjectID { get; private set; }

        public float PosX { get; private set; }
        public float PosY { get; private set; }
        public float PosZ { get; private set; }

        public int ObjectDimension { get; private set; }

        public void Init(string prefabName, int id, int owner, bool destroyOnOwnerDisconnect = false)
        {
            OwnerClientID = owner;
            ObjectID = id;
            PrefabName = prefabName;
            DestroyOnOwnerDisconnect = destroyOnOwnerDisconnect;
        }

        public void UpdatePosition(float x, float y, float z)
        {
            PosX = x;
            PosY = y;
            PosZ = z;
        }

        public void UpdateDimension(int d)
        {
            ObjectDimension = d;
        }
    }
}
