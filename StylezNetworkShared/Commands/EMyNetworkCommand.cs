using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StylezNetwork.Commands
{
    public enum EMyNetworkCommand
    {
        COMMAND_AUTH,
        COMMAND_OBJECT_CREATE,
        COMMAND_WORLD_GETOBJECTS,
        COMMAND_MOVE_OBJECT,
        COMMAND_REQUEST_AREAUPDATE,
        COMMAND_OBJECT_DELETE,
        COMMAND_PING
    }
}
