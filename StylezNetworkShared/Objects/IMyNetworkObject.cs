﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StylezNetwork.MathEx;

namespace StylezNetwork.Objects
{
    public interface IMyNetworkObject
    {
        Vector3 Position { get; }
        int Dimension { get; }
        int OwnerClientID { get; }
        int ObjectNetworkID { get; }

        void UpdatePosition(Vector3 newPosition);
        void UpdateDimension(int dim);
    }
}
