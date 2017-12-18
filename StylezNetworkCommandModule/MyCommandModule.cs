using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StylezNetworkCommandModule
{
    public class MyCommandModule
    {
        public MyCommandModule(EMyModuleSide side)
        {

        }
    }

    public enum EMyModuleSide
    {
        SERVER,
        CLIENT
    }
}
