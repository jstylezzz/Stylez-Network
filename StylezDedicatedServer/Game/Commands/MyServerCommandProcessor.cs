using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StylezDedicatedServer.Core;
using StylezNetworkShared.Network;
using StylezNetworkShared.Commands;
using StylezNetworkShared.Game.Commands;
using Newtonsoft.Json;

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
        /// <param name="message">The NetCommand that was received.</param>
        private void OnTransmissionReceived(MyNetworkClient fromClient, MyNetCommand cmd)
        {
            //Commands that may be performed while the client provides
            //correct authentication.
            if (fromClient.IsAuthenticated && fromClient.AuthCode.Equals(cmd.AuthCode))
            {
                
            }
            else if(!fromClient.IsAuthenticated) //Commands that may be performed while not authenticated
            {
                switch ((EMyNetworkCommands)cmd.CommandID)
                {
                    case EMyNetworkCommands.AUTHENTICATE:
                    {
                        fromClient.SendTransmission(new MyNetCommand((int)EMyNetworkCommands.AUTHENTICATE, JsonConvert.SerializeObject(new MyClientAuthCommand(fromClient.ClientID, fromClient.AuthCode))));
                        fromClient.SetAuthenticated(true);
                        break;
                    }
                    default:
                    {
                        //Disconnect client, they should authenticate when sending messages..
                        
                        break;
                    }
                }
            }
            else //Client is authenticated BUT provided wrong authcode. Disconnect to be sure.
            {
                
            }
        }
    }
}
