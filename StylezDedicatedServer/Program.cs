using System;
using System.Threading;
using StylezDedicatedServer.Core;
using StylezNetworkShared.Logging;
using StylezDedicatedServer.Events;

namespace StylezDedicatedServer
{
    class Program
    {
        private static ManualResetEvent QuitEvent = new ManualResetEvent(false);
        static void Main(string[] args)
        {
            new MyDedicatedServer();

            Console.CancelKeyPress += (sender, e) =>
            {
                MyEventManager.Instance.FireShutdownEvent();
                QuitEvent.Set();
                e.Cancel = true;
            };

            MyLogger.LogInfo("Press CTRL+C to stop the server..");
            QuitEvent.WaitOne();
        }
    }
}
