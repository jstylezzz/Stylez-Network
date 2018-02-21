using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StylezNetworkShared.Game.World.Objects
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
    }
}
