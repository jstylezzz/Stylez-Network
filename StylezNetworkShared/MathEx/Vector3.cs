using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StylezNetwork.MathEx
{
    public class Vector3
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }

        public static Vector3 Zero { get { return new Vector3(0, 0, 0); } }
        public static Vector3 Left { get { return new Vector3(-1, 0, 0); } }
        public static Vector3 Right { get { return new Vector3(1, 0, 0); } }
        public static Vector3 Up { get { return new Vector3(0, 1, 0); } }
        public static Vector3 Down { get { return new Vector3(0, -1, 0); } }
        public static Vector3 Forward { get { return new Vector3(0, 0, 1); } }
        public static Vector3 Backward { get { return new Vector3(0, 0, -1); } }

        public Vector3()
        {
            this.x = 0f;
            this.y = 0f;
            this.z = 0f;
        }

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static float Distance(Vector3 a, Vector3 b)
        {
            return (float)Math.Sqrt(Math.Pow(a.x - b.x, 2) + Math.Pow(a.y - b.y, 2) + Math.Pow(a.z - b.z, 2));
        }
    }
}
