using System;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StylezNetworkShared.Network
{
    public static class MyNetPacketUtil
    {
        /*
         * A packet is built up as follows:
         * 
         * 4 bytes > int > transmission length
         * 8 bytes > string > authCode
         * ?? bytes > string > command
         */

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
            //Start at 12 because we need to skip 4 bytes for length.
            return BitConverter.ToString(buf, 4);
        }

        public static string GetMessageFromBuf(byte[] buf)
        {
            //Skip the first 12 bytes because: 4 for transmission length,
            //8 for authcode.
            return Encoding.ASCII.GetString(buf);
        }

        /// <summary>
        /// Pack a message that can be read by the StylezNetwork receivers.
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="authCode">Authcode to use</param>
        /// <returns>NetPacket instance with the transmission and its length.</returns>
        public static MyNetPacket PackMessage(string message, string authCode)
        {
            if(authCode.Length > 8)
            {
                int excess = authCode.Length - 8;
                authCode = authCode.Remove(8, excess);
            }


            byte[] mes = Encoding.ASCII.GetBytes(message);
            byte[] code = Encoding.ASCII.GetBytes(authCode);
            byte[] len = BitConverter.GetBytes(mes.Length);
            byte[] packedMessage = new byte[4 + 8 + mes.Length];

            len.CopyTo(packedMessage, 0);
            code.CopyTo(packedMessage, 4);
            mes.CopyTo(packedMessage, 12);
            return new MyNetPacket(packedMessage, packedMessage.Length);
        }
    }
}
