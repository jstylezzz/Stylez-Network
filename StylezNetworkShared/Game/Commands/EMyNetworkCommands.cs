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
        /// This is reserved for errors.
        /// </summary>
        ERROR,

        /// <summary>
        /// Serverside: Request for an auth code and client ID.
        /// Clientside: Register received client ID and auth code.
        /// </summary>
        AUTHENTICATE,

        /// <summary>
        /// Serverside: Register new object to the world manager.
        /// Clientside: Nothing.
        /// </summary>
        SPAWN_OBJECT,

        /// <summary>
        /// Serverside: Send the list of objects in range to the client.
        /// Clientside: Update existing objects, spawn new objects, delete removed objects.
        /// </summary>
        WORLD_AREA_UPDATE
    }
}
