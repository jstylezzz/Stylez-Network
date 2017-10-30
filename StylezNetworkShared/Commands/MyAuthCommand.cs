using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StylezNetwork.Commands
{
    public class MyAuthCommand
    {
        public string AuthCode { get; set; }
        public int ClientID { get; set; }
        public EMyNetworkCommand AssociatedCommand { get { return EMyNetworkCommand.COMMAND_AUTH; } }

        public MyAuthCommand(string code, int id)
        {
            AuthCode = code;
            ClientID = id;
        }
    }
}
