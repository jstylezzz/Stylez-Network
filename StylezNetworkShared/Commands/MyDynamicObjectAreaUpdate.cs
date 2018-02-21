using Newtonsoft.Json;
using StylezNetworkShared.Objects;
using System;

namespace StylezNetworkShared.Commands
{
    [Serializable]
    public class MyDynamicObjectAreaUpdate
    {
        /// <summary>
        /// Amount of dynamic objects in range.
        /// </summary>
        public int DynamicObjectCount { get; set; }

        public MyDynamicObject[] DynamicObjects { get; set; }

        [JsonConstructor]
        public MyDynamicObjectAreaUpdate(int amount, MyDynamicObject[] objects)
        {
            DynamicObjectCount = amount;
            DynamicObjects = objects;
        }
    }
}
