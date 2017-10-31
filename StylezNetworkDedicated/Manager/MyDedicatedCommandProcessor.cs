using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using StylezNetwork.Commands;
using StylezNetwork.Objects;

namespace StylezNetworkDedicated.Manager
{
    public class MyDedicatedCommandProcessor
    {
        public void ProcessCommand(int cmdId, string cmdJson, int fromClient)
        {
            switch ((EMyNetworkCommand)Enum.Parse(typeof(EMyNetworkCommand), cmdId.ToString()))
            {
                case EMyNetworkCommand.COMMAND_OBJECT_CREATE:
                {
                    MyCreateObjectCommand cmd = JsonConvert.DeserializeObject<MyCreateObjectCommand>(cmdJson);
                    Program.Instance.WorldCacheInstance.AddObjectToWorld(new MySimpleWorldObject(cmd.AtPosition, cmd.InDimension, Program.Instance.WorldCacheInstance.GetFirstFreeObjectID(true), fromClient));
                    break;
                }
                case EMyNetworkCommand.COMMAND_WORLD_GETOBJECTS:
                {
                    MyRequestAllObjectsCommand requestCommand = JsonConvert.DeserializeObject<MyRequestAllObjectsCommand>(cmdJson);
                    IMyNetworkObject[] objectsToSend = Program.Instance.WorldCacheInstance.GetNetworkObjects(requestCommand.LoadingPoint, requestCommand.LoadingDimension, requestCommand.LoadingDistance);
                    MyRequestAllObjectsCommand outCommand = new MyRequestAllObjectsCommand(objectsToSend);
                    Program.Instance.ServerBaseInstance.ClientRegistry[fromClient].SendMessage(JsonConvert.SerializeObject(outCommand), (int)EMyNetworkCommand.COMMAND_WORLD_GETOBJECTS);
                    break;
                }
            }
        }
    }
}
