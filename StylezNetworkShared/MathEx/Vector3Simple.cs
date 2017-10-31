using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StylezNetwork.MathEx
{
    [Serializable]
    public class Vector3Simple
    {
        public double x;
        public double y;
        public double z;

        public static Vector3Simple Zero { get { return new Vector3Simple(0, 0, 0); } }
        public static Vector3Simple Left { get { return new Vector3Simple(-1, 0, 0); } }
        public static Vector3Simple Right { get { return new Vector3Simple(1, 0, 0); } }
        public static Vector3Simple Up { get { return new Vector3Simple(0, 1, 0); } }
        public static Vector3Simple Down { get { return new Vector3Simple(0, -1, 0); } }
        public static Vector3Simple Forward { get { return new Vector3Simple(0, 0, 1); } }
        public static Vector3Simple Backward { get { return new Vector3Simple(0, 0, -1); } }

        public Vector3Simple()
        {
            this.x = 0d;
            this.y = 0d;
            this.z = 0d;
        }

        public Vector3Simple(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static double Distance(Vector3Simple a, Vector3Simple b)
        {
            return Math.Sqrt(Math.Pow(a.x - b.x, 2) + Math.Pow(a.y - b.y, 2) + Math.Pow(a.z - b.z, 2));
        }
    }
}
