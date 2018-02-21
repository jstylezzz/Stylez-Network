using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StylezDedicatedServer.Threading;
using StylezNetworkShared.Logging;
using StylezDedicatedServer.Network;
using StylezDedicatedServer.Events;
using StylezDedicatedServer.Game.Commands;
using StylezDedicatedServer.Game.Manager;
using StylezNetworkShared.Manager;

namespace StylezDedicatedServer.Core
{
    public class MyDedicatedServer
    {
        public static MyObjectManager ServerObjectManager { get; private set; }
        
        public MyDedicatedServer()
        {
            MyLogger.LogInfo("Initializing server..");
            InitializeSingleInstances();
            InitializeGameSpecificInstances();
            new MyClientListener("127.0.0.1", 7788);
        }

        /// <summary>
        /// Initializes classes that are game specific; Not part of the
        /// server core.
        /// </summary>
        private void InitializeGameSpecificInstances()
        {
            new MyServerCommandProcessor();
            ServerObjectManager = new MyObjectManager();
            new MyClientWorldManager();
        }

        /// <summary>
        /// Initialize classes that are part of the server core.
        /// </summary>
        private void InitializeSingleInstances()
        {
            new MyEventManager();
            new MyThreadManager();
            new MyClientManager();
        }
    }
}
