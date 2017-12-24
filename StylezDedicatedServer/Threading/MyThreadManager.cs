using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using StylezNetworkShared.Logging;
using StylezDedicatedServer.Events;

namespace StylezDedicatedServer.Threading
{
    public class MyThreadManager
    {
        public static MyThreadManager Instance { get; private set; }
        public bool ThreadingRunning { get { return m_running; } }

        private bool m_running = true;
        private Queue<Action> m_clientListenerThreadQueue = new Queue<Action>();

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
     
        }

        private void OnServerShutdown()
        {
            m_running = false;
        }
    }
}
