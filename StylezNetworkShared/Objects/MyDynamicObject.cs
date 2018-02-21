using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StylezNetworkShared.Objects
{
    public class MyDynamicObject : MyObjectBase
    {
        public float VelocityX { get; private set; }
        public float VelocityY { get; private set; }
        public float VelocityZ { get; private set; }

        public float AngularDrag { get; private set; }
        public float Drag { get; private set; }
        public float Mass { get; private set; }

        public float AngularVelocityX { get; private set; }
        public float AngularVelocityY { get; private set; }
        public float AngularVelocityZ { get; private set; }

        public void UpdateAngularDrag(float ad)
        {
            AngularDrag = ad;
        }

        public void UpdateDrag(float d)
        {
            Drag = d;
        }

        public void UpdateMass(float m)
        {
            Mass = m;
        }

        public void UpdateVelocity(float vx, float vy, float vz)
        {
            VelocityX = vx;
            VelocityY = vy;
            VelocityZ = vz;
        }

        public void UpdateAngularVelocity(float avx, float avy, float avz)
        {
            AngularVelocityX = avx;
            AngularVelocityY = avy;
            AngularVelocityZ = avz;
        }

        /// <summary>
        /// Get the distance from this object to a point.
        /// </summary>
        /// <param name="x">X position of the point.</param>
        /// <param name="y">Y position of the point.</param>
        /// <param name="z">Z position of the point.</param>
        /// <returns>A float with the distance.</returns>
        public float DistanceTo(float x, float y, float z)
        {
            return (float)(Math.Sqrt(Math.Pow(Math.Abs(PosX - x), 2) + Math.Pow(Math.Abs(PosY - y), 2) + Math.Pow(Math.Abs(PosZ - z), 2)));
        }
    }
}
