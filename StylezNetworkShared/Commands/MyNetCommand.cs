using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StylezNetworkShared.Commands
{
    public struct MyNetCommand
    {
        /// <summary>
        /// JSON string of a command class.
        /// </summary>
        public string CommandJSON { get; }

        /// <summary>
        /// Command ID that the CommandJSON is for.
        /// </summary>
        public int CommandID { get; }

        /// <summary>
        /// The authcode that this command was sent with.
        /// </summary>
        public string AuthCode { get; set; }

        /// <summary>
        /// Set up a new NetCommand instance with command JSON data.
        /// </summary>
        /// <param name="ID">The ID of the command.</param>
        /// <param name="cmd">The JSON string of the command.</param>
        public MyNetCommand(int ID, string cmd)
        {
            CommandID = ID;
            CommandJSON = cmd;
            AuthCode = "NOAUTHCD";
        }

        /// <summary>
        /// Set up a new NetCommand instance without command JSON data.
        /// </summary>
        /// <param name="ID">The ID of the command.</param>
        public MyNetCommand(int ID)
        {
            CommandID = ID;
            CommandJSON = " ";
            AuthCode = "NOAUTHCD";
        }
    }
}
