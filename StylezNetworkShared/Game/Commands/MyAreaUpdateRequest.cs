using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StylezNetworkShared.Game.Commands
{
    [Serializable]
    public struct MyAreaUpdateRequest
    {
        public int ForClientID;
        public float PosX;
        public float PosY;
        public float PosZ;
        public int Dimension;
        public float StreamDistance;

        public MyAreaUpdateRequest(int forclient, float x, float y, float z, int dimension, float streamdist)
        {
            ForClientID = forclient;
            PosX = x;
            PosY = y;
            PosZ = z;
            Dimension = dimension;
            StreamDistance = streamdist;
        }
    }
}
