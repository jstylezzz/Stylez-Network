using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StylezNetwork.MathEx;

namespace StylezNetwork.Objects
{
    public interface IMyNetworkObject
    {
        Vector3Simple Position { get; }
        int Dimension { get; }
        int OwnerClientID { get; }
        int ObjectNetworkID { get; }
        EMyObjectType ObjectType { get; }

        void UpdatePosition(Vector3Simple newPosition);
        void UpdateDimension(int dim);
    }
}
