using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StylezNetwork.MathEx;

namespace StylezNetwork.Objects
{
    public class MyPlayerEntityWorldObject : IMyNetworkObject
    {
        public Vector3Simple Position { get; private set; }

        public int Dimension { get; private set; }

        public int OwnerClientID { get; private set; }

        public int ObjectNetworkID { get; private set; }

        public MyPlayerEntityWorldObject(Vector3Simple pos, int dim)
        {
            Position = pos;
            Dimension = dim;
        }

        public void UpdateDimension(int dim)
        {
            Dimension = dim;
        }

        public void UpdatePosition(Vector3Simple newPosition)
        {
            Position = newPosition;
        }
    }
}
