using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StylezNetwork.Objects;

namespace StylezNetwork.Commands
{
    [Serializable]
    public class MyMoveObjectCommand
    {
        public MyObjectMovementData MovementData;
        public int ObjectID;
    }
}
