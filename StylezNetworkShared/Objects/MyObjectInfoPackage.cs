using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StylezNetwork.MathEx;

namespace StylezNetwork.Objects
{
    public class MyObjectInfoPackage
    {
        public int ObjectID { get; private set; }
        public Vector3Simple ObjectPosition { get; private set; }
        public MyObjectMovementData MovementData { get; private set; }
        public EMyObjectType ObjectType { get; private set; }
        public int OwnerID { get; private set; }

        public MyObjectInfoPackage(int id, Vector3Simple pos, MyObjectMovementData movement, EMyObjectType type, int owner)
        {
            this.ObjectID = id;
            this.ObjectPosition = pos;
            this.MovementData = movement;
            this.ObjectType = type;
            this.OwnerID = owner;
        }
    }
}
