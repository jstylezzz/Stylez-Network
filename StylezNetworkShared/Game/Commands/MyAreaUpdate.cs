using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StylezNetworkShared.Game.World.Objects;
using Newtonsoft.Json;

namespace StylezNetworkShared.Game.Commands
{
    [Serializable]
    public struct MyAreaUpdate
    {
        public int ObjectAmount;
        public MyWorldObject[] WorldObjects;

        [JsonConstructor]
        public MyAreaUpdate(int amnt, MyWorldObject[] objs)
        {
            ObjectAmount = amnt;
            WorldObjects = objs;
        }
    }
}
