using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StylezNetwork.Commands
{
    public class MyAuthCommand
    {
        public string AuthCode;
        public int ClientID;

        public MyAuthCommand(string code, int id)
        {
            AuthCode = code;
            ClientID = id;
        }
    }
}
