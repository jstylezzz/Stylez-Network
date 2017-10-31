using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StylezNetwork.MathEx;

namespace StylezNetwork.Commands
{
    public class MyCreateObjectCommand
    {
        public Vector3Simple AtPosition;
        public int InDimension;

        public MyCreateObjectCommand(Vector3Simple atPosition, int inDimension)
        {
            this.AtPosition = atPosition;
            this.InDimension = inDimension;
        }
    }
}
