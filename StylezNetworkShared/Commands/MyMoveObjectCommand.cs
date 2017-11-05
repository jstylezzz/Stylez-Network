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

        public MyMoveObjectCommand()
        {
            //Empty json constructor
        }

        public MyMoveObjectCommand(MyObjectMovementData moveData, int id)
        {
            MovementData = moveData;
            ObjectID = id;
        }
    }
}
