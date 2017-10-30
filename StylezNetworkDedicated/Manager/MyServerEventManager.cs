using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StylezNetworkDedicated.Manager
{
    public class MyServerEventManager
    {
        public delegate void OnServerReadyEventDelegate();

        public static OnServerReadyEventDelegate OnServerReady { get; set; }
    }
}
