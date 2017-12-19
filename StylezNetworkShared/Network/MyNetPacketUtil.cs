using System;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StylezNetworkShared.Commands;

namespace StylezNetworkShared.Network
{
    public static class MyNetPacketUtil
    {
        /*
         * A packet is built up as follows:
         * 
         * 4 bytes > int > transmission length
         * 8 bytes > string > authCode
         * 4 bytes > int > commandID from command enum
         * ?? bytes > string > command
         */

        /// <summary>
        /// The initial stream read length for client messages.
        /// These bytes contain info about the transmission content.
        /// </summary>
        public const int InitialTransmissionLength = 12;

        /// <summary>
        /// Get transmission length from message buffer.
        /// </summary>
        /// <param name="buf">The buffer to read from.</param>
        /// <returns>Length of the received transmission.</returns>
        public static int GetTransmissionLength(byte[] buf)
        {
            return BitConverter.ToInt32(buf, 0);
        }

        /// <summary>
        /// Get the auth code from a message buffer.
        /// </summary>
        /// <param name="buf">Buffer to get the code from.</param>
        /// <returns>The auth code in the message.</returns>
        public static string GetAuthCodeFromBuf(byte[] buf)
        {
            //We read 8 bytes for the auth code.
            return Encoding.ASCII.GetString(buf, 4, 8);
        }

        /// <summary>
        /// Get a NetCommand instance from the specified buffer.
        /// </summary>
        /// <param name="buf">The buffer to retrieve the NetCommand from.</param>
        /// <returns>A NetCommand instance with command data (ID and JSON).</returns>
        public static MyNetCommand GetCommandFromBuf(byte[] buf)
        {
            //First 4 bytes are command ID, rest of the bytes are JSON.
            return new MyNetCommand(BitConverter.ToInt32(buf, 0), Encoding.ASCII.GetString(buf, 4, buf.Length - 4));
        }

        /// <summary>
        /// Pack a message that can be read by the StylezNetwork receivers.
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="authCode">Authcode to use</param>
        /// <returns>NetPacket instance with the transmission and its length.</returns>
        public static MyNetPacket PackMessage(int commandID, string message, string authCode)
        {
            if(authCode.Length > 8)
            {
                int excess = authCode.Length - 8;
                authCode = authCode.Remove(8, excess);
            }

            //Set up byte arrays containing our data
            byte[] cmdID = BitConverter.GetBytes(commandID);
            byte[] mes = Encoding.ASCII.GetBytes(message);
            byte[] code = Encoding.ASCII.GetBytes(authCode);

            //Message length + cmd id len
            byte[] len = BitConverter.GetBytes(mes.Length + cmdID.Length);

            //Calculate total length by adding 4 bytes for length, 8 bytes for authcode, 4 bytes for CMD ID, ?? bytes for message length
            byte[] packedMessage = new byte[4 + 8 + 4 + mes.Length]; 

            len.CopyTo(packedMessage, 0); //Len takes 4 bytes (Int32)
            code.CopyTo(packedMessage, 4); //Code takes 8 bytes (string, length of 8)
            cmdID.CopyTo(packedMessage, 12); //Command ID takes 4 bytes (Int32)
            mes.CopyTo(packedMessage, 16); //Rest of the message takes ?? bytes (string)
            return new MyNetPacket(packedMessage, packedMessage.Length);
        }
    }
}
