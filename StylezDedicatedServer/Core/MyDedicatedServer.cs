using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StylezDedicatedServer.Threading;
using StylezNetworkShared.Logging;
using StylezDedicatedServer.Network;
using StylezDedicatedServer.Events;

namespace StylezDedicatedServer.Core
{
    public class MyDedicatedServer
    {
        public MyDedicatedServer()
        {
            MyLogger.LogInfo("Initializing server..");
            InitializeSingleInstances();
            new MyClientListener("127.0.0.1", 7788);
        }

        private void InitializeSingleInstances()
        {
            new MyEventManager();
            new MyThreadManager();
            new MyClientManager();
        }
    }
}
