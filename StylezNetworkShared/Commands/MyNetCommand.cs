using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StylezNetworkShared.Commands
{
    public struct MyNetCommand
    {
        public string CommandJSON { get; }
        public int CommandID { get; }

        public MyNetCommand(int ID, string cmd)
        {
            CommandID = ID;
            CommandJSON = cmd;
        }
    }
}
