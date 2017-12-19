using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StylezNetworkShared.Game.Commands
{
    /// <summary>
    /// Enum containing all game specific commands.
    /// </summary>
    public enum EMyNetworkCommands
    {
        /// <summary>
        /// Serverside: Request for an auth code and client ID.
        /// Clientside: Register received client ID and auth code.
        /// </summary>
        AUTHENTICATE
    }
}
