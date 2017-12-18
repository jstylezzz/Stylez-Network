using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace StylezNetworkShared.Network
{
    public struct MyWorkingSocket
    {
        public Socket SocketInstance { get; set; }
        public byte[] Buffer { get; set; }
        public int TransmissionLength { get; set; }
        public string ReceivedAuthCode { get; set; }
    }
}
