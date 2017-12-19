using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StylezDedicatedServer.Core;
using StylezNetworkShared.Network;
using StylezNetworkShared.Commands;

namespace StylezDedicatedServer.Game.Commands
{
    /// <summary>
    /// Serverside command processor. This class is separate from the 
    /// server core, and should be written to match the game its for.
    /// </summary>
    public class MyServerCommandProcessor
    {
        public static MyServerCommandProcessor Instance { get; private set; }       

        public MyServerCommandProcessor()
        {
            if (Instance == null)
            {
                Instance = this;
                MyClientManager.Instance.OnTransmissionReceive += OnTransmissionReceived;
            }
        }

        /// <summary>
        /// Called when the dedicated server receives a message from a client.
        /// </summary>
        /// <param name="fromClient">The NetClient instance of the receiving client.</param>
        /// <param name="message">The JSON message that was received.</param>
        private void OnTransmissionReceived(MyNetworkClient fromClient, MyNetCommand message)
        {
            
        }
    }
}
