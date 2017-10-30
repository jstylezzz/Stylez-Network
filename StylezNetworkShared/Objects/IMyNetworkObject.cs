using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StylezNetwork.MathEx;

namespace StylezNetwork.Objects
{
    interface IMyNetworkObject
    {
        Vector3 Position { get; }
        int Dimension { get; }
        int OwnerClientID { get; }

        void UpdatePosition(Vector3 newPosition);
        void UpdateDimension(int dim);
    }
}
