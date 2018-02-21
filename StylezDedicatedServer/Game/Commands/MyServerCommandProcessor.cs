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
using StylezNetworkShared.Logging;
using StylezDedicatedServer.Game.Manager;
using StylezNetworkShared.Objects;

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
                switch ((EMyNetworkCommands)cmd.CommandID)
                {
                    case EMyNetworkCommands.SPAWN_DYNAMIC_OBJECT:
                    {
                        MyDynamicObject obj = JsonConvert.DeserializeObject<MyDynamicObject>(cmd.CommandJSON);
                        obj.UpdateOwnerClientID(fromClient.ClientID); //We set this here to prevent spoofing from the other end.
                        MyDedicatedServer.ServerObjectManager.RegisterDynamicObject(obj); //Register the object to the world manager. It will auto-broadcast when clients perform area updates.
                        
                        break;
                    }
                    case EMyNetworkCommands.WORLD_AREA_UPDATE:
                    {
                        MyClientWorldManager.Instance.PerformClientAreaUpdate(JsonConvert.DeserializeObject<MyAreaUpdateRequest>(cmd.CommandJSON));
                        break;
                    }
                    case EMyNetworkCommands.MAKE_DYNAMIC_WORLD_AREA_UPDATE:
                    {
                        MyDynamicObjectAreaUpdate aud = JsonConvert.DeserializeObject<MyDynamicObjectAreaUpdate>(cmd.CommandJSON);
                        for (int i = 0, len = aud.DynamicObjectCount; i < len; i++)
                        {
                            MyDedicatedServer.ServerObjectManager.UpdateDynamicObject(aud.DynamicObjects[i]);
                        }

                        break;
                    }
                    case EMyNetworkCommands.PERFORM_PING:
                    {
                        fromClient.SendTransmission(new MyNetCommand((int)EMyNetworkCommands.PERFORM_PING, " "));
                        break;
                    }
                    default:
                    {
                        MyLogger.LogError($"Client ID {fromClient.ClientID} attempted to execute an invalid command (ID: {cmd.CommandID}).");
                        break;
                    }
                }
            }
            else if (!fromClient.IsAuthenticated) //Commands that may be performed while not authenticated
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
                        fromClient.Disconnect();
                        break;
                    }
                }
            }
            else //Client is authenticated BUT provided wrong authcode. Disconnect to be sure.
            {
                fromClient.Disconnect();
            }
        }
    }
}
