using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StylezNetwork.Objects
{
    [Serializable]
    public class MyNetworkObjectPositionData
    {
        public double X;
        public double Y;
        public double Z;

        public MyNetworkObjectPositionData(double xpos, double ypos, double zpos)
        {
            X = xpos;
            Y = ypos;
            Z = zpos;
        }
    }
}
