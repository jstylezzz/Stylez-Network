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
        public void ProcessCommand(int cmdId, string cmdJson)
        {
            switch ((EMyNetworkCommand)Enum.Parse(typeof(EMyNetworkCommand), cmdId.ToString()))
            {
                case EMyNetworkCommand.COMMAND_OBJECT_CREATE:
                {
                    MyCreateObjectCommand cmd = JsonConvert.DeserializeObject<MyCreateObjectCommand>(cmdJson);
                    Program.Instance.WorldCacheInstance.AddObjectToWorld(new MyPlayerEntityWorldObject(cmd.AtPosition, cmd.InDimension));
                    break;
                }
            }
        }
    }
}
