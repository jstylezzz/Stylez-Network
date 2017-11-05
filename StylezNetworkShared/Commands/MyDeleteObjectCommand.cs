using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StylezNetwork.Commands
{
    [Serializable]
    public class MyDeleteObjectCommand
    {
        public int ObjectID;

        public MyDeleteObjectCommand(int id)
        {
            this.ObjectID = id;
        }
    }
}
