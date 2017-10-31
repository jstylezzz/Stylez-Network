using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StylezNetwork.MathEx
{
    public class Vector3Simple
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }

        public static Vector3Simple Zero { get { return new Vector3Simple(0, 0, 0); } }
        public static Vector3Simple Left { get { return new Vector3Simple(-1, 0, 0); } }
        public static Vector3Simple Right { get { return new Vector3Simple(1, 0, 0); } }
        public static Vector3Simple Up { get { return new Vector3Simple(0, 1, 0); } }
        public static Vector3Simple Down { get { return new Vector3Simple(0, -1, 0); } }
        public static Vector3Simple Forward { get { return new Vector3Simple(0, 0, 1); } }
        public static Vector3Simple Backward { get { return new Vector3Simple(0, 0, -1); } }

        public Vector3Simple()
        {
            this.x = 0f;
            this.y = 0f;
            this.z = 0f;
        }

        public Vector3Simple(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static float Distance(Vector3Simple a, Vector3Simple b)
        {
            return (float)Math.Sqrt(Math.Pow(a.x - b.x, 2) + Math.Pow(a.y - b.y, 2) + Math.Pow(a.z - b.z, 2));
        }
    }
}
