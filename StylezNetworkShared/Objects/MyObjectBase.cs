using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace StylezNetworkShared.Objects
{
    [Serializable]
    public class MyObjectBase
    {
        [JsonProperty]
        public string PrefabName { get; private set; }

        [JsonProperty]
        public EMyObjectType ObjectType { get; private set; }

        [JsonProperty]
        public bool DestroyOnOwnerDisconnect { get; private set; }

        [JsonProperty]
        public int OwnerClientID { get; private set; }

        [JsonProperty]
        public int ObjectID { get; private set; }


        [JsonProperty]
        public float PosX { get; private set; }

        [JsonProperty]
        public float PosY { get; private set; }

        [JsonProperty]
        public float PosZ { get; private set; }

        [JsonProperty]
        public int ObjectDimension { get; private set; }

        public void Init(string prefabName, EMyObjectType type, int id, int owner, bool destroyOnOwnerDisconnect = false)
        {
            OwnerClientID = owner;
            ObjectID = id;
            PrefabName = prefabName;
            DestroyOnOwnerDisconnect = destroyOnOwnerDisconnect;
            ObjectType = type;
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

        public void UpdateOwnerClientID(int id)
        {
            OwnerClientID = id;
        }

        public void SetObjectID(int id)
        {
            ObjectID = id;
        }

        public void SetObjectPrefabName(string pf)
        {
            PrefabName = pf;
        }
    }
}
