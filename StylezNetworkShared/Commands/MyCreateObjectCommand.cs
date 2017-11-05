using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StylezNetwork.MathEx;
using StylezNetwork.Objects;

namespace StylezNetwork.Commands
{
    public class MyCreateObjectCommand
    {
        public Vector3Simple AtPosition;
        public int InDimension;
        public int ClientIDOwner;
        public EMyObjectType ObjectType;

        public MyCreateObjectCommand(Vector3Simple atPosition, int inDimension, int ownerid, EMyObjectType ot)
        {
            this.AtPosition = atPosition;
            this.InDimension = inDimension;
            this.ClientIDOwner = ownerid;
            this.ObjectType = ot;
        }
    }
}
