using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StylezNetworkShared.Network;
using StylezNetworkShared.Commands;

namespace StylezNetworkClientDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program();
        }

        public Program()
        {
            MyNetworkClient nc = new MyNetworkClient(EMyNetClientMode.MODE_CLIENT);
            nc.OnTransmissionReceived += TransReceived;
            nc.ConnectToServer("127.0.0.1", 7788);
            Console.ReadKey();
        }

        private void TransReceived(MyNetworkClient c, MyNetCommand nc)
        {
            Console.WriteLine($"Received command ID {nc.CommandID} with string {nc.CommandJSON}.");
        }
    }
}
