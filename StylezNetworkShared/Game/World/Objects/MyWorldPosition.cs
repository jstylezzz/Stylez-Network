using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StylezNetworkShared.Game.World.Objects
{
    [Serializable]
    public class MyWorldPosition
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
        public int Dimension { get; set; }

        public MyWorldPosition(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            Dimension = 0;
        }

        public MyWorldPosition(float x, float y, float z, int dimension)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            Dimension = dimension;
        }

        public MyWorldPosition(int dimension)
        {
            x = 0f;
            y = 0f;
            z = 0f;
            Dimension = dimension;
        }

        public void UpdateDimension(int dim)
        {
            Dimension = dim;
        }

        public void UpdatePosition(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public MyWorldPosition()
        {
           //Json constructor
        }
    }
}
