﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StylezNetworkDedicated.Network;
using StylezNetworkDedicated.Manager;
using System.Threading;

namespace StylezNetworkDedicated
{
    class Program
    {
        private static ManualResetEvent m_quitEvent = new ManualResetEvent(false);
        static void Main(string[] args)
        {
            new Program(args);
        }


        private MyDedicatedServerBase m_dediBase;

        public Program(string[] arguments)
        {
            MyServerEventManager.OnServerReady += OnServerReady;
            SetupDediBase(arguments);
            Console.CancelKeyPress += (sender, e) =>
            {
                m_quitEvent.Set();
                e.Cancel = true;
            };

            m_quitEvent.WaitOne();
            Console.WriteLine("[INFO]: Server stopping..");
            m_dediBase.StopServer();
            Console.WriteLine("[INFO]: Server stopped..");
        }

        private void OnServerReady()
        {
            Console.WriteLine("[INFO]: Server started. Press CTRL+C to close.");
        }

        private void SetupDediBase(string[] arguments)
        {
            string ip;
            int port;
            if (arguments.Length < 2)
            {
                ip = "127.0.0.1";
                port = 7777;
            }
            else
            {
                ip = arguments[0];
                if (!Int32.TryParse(arguments[1], out port))
                {
                    Console.WriteLine("[WARN]: Something went wrong while parsing the port. Default 7777 will be used.");
                    port = 7777;
                }
            }
            m_dediBase = new MyDedicatedServerBase(ip, port);
        }
    }
}
