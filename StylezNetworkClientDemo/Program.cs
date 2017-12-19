﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StylezNetworkShared.Network;
using StylezNetworkShared.Commands;
using StylezNetworkShared.Game.Commands;
using Newtonsoft.Json;

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
            nc.OnDisconnect += OnDisconnect;
            nc.ConnectToServer("127.0.0.1", 7788);
            nc.SendTransmission(new MyNetCommand((int)EMyNetworkCommands.AUTHENTICATE));
            Console.WriteLine("Press any key to disconnect..");
            Console.ReadKey();
            nc.Disconnect();
            Console.ReadKey();
        }

        private void OnDisconnect(int clientid)
        {
            Console.WriteLine("Disconnected from the server. Press any key to quit.");
        }

        private void TransReceived(MyNetworkClient c, MyNetCommand nc)
        {
            Console.WriteLine($"Received command ID {nc.CommandID} with string {nc.CommandJSON}.");
            switch ((EMyNetworkCommands)nc.CommandID)
            {
                case EMyNetworkCommands.AUTHENTICATE:
                {
                    MyClientAuthCommand cmd = JsonConvert.DeserializeObject<MyClientAuthCommand>(nc.CommandJSON);
                    c.AuthClient(cmd.ClientID, cmd.AuthCode);
                    c.SetAuthenticated(true);
                    break;
                }
            }
        }
    }
}
