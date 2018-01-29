using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using StylezNetworkShared.Logging;
using StylezDedicatedServer.Events;
using System.Diagnostics;

namespace StylezDedicatedServer.Threading
{
    public class MyThreadManager
    {
        public static MyThreadManager Instance { get; private set; }
        public bool ThreadingRunning { get { return m_running; } }

        private bool m_running = true;

        public MyThreadManager()
        {
            if (Instance == null)
            {
                Instance = this;
                MyEventManager.Instance.OnServerShutdown += OnServerShutdown;
                StartThreads();
            }
            else MyLogger.LogError("There is already a thread manager created. Instance dropped.");
        }

        private void StartThreads()
        {
            new Thread(ThreadedGameLogicLoop).Start();
        }

        /// <summary>
        /// We run custom game logic from a separate thread.
        /// </summary>
        private void ThreadedGameLogicLoop()
        {
            while(m_running)
            {     
                MyEventManager.Instance.PerformGameLogicUpdate();
                Thread.Sleep(1000);
            }
        }

        private void OnServerShutdown()
        {
            m_running = false;
        }
    }
}
