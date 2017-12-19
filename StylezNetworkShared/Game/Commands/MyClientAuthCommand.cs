using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StylezNetworkShared.Game.Commands
{
    [System.Serializable]
    public struct MyClientAuthCommand
    {
        public int ClientID;
        public string AuthCode;

        public MyClientAuthCommand(int id, string code)
        {
            ClientID = id;
            AuthCode = code;
        }
    }
}
