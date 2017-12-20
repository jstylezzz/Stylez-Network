using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StylezNetworkShared.Game.World.Objects;

namespace StylezNetworkShared.Game.Commands
{
    [Serializable]
    public struct MyAreaUpdate
    {
        public int ObjectAmount { get; }
        public MyWorldObject[] WorldObjects { get; }

        public MyAreaUpdate(int amnt, MyWorldObject[] objs)
        {
            ObjectAmount = amnt;
            WorldObjects = objs;
        }
    }
}
