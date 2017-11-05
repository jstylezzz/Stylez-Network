using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StylezNetwork.MathEx;

namespace StylezNetwork.Objects
{
    public class MySimpleWorldObject : IMyNetworkObject
    {
        public Vector3Simple Position { get; private set; }

        public int Dimension { get; private set; }

        public int OwnerClientID { get; private set; }

        public int ObjectNetworkID { get; private set; }

        public EMyObjectType ObjectType { get; private set; }

        public MySimpleWorldObject(Vector3Simple pos, int dim, int networkID, int clientID, EMyObjectType t)
        {
            Position = pos;
            Dimension = dim;
            OwnerClientID = clientID;
            ObjectNetworkID = networkID;
            ObjectType = t;
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
