using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StylezNetworkShared.Network
{
    public struct MyNetPacket
    {
        public byte[] Transmission { get; private set; }
        public int TransmissionLength { get; private set; }

        public MyNetPacket(byte[] t, int l)
        {
            Transmission = t;
            TransmissionLength = l;
        }
    }
}
