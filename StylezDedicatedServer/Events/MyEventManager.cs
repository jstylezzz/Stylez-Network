﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StylezNetworkShared.Logging;

namespace StylezDedicatedServer.Events
{

    public delegate void OnServerShutdownEventDelegate();

    public class MyEventManager
    {
        public static MyEventManager Instance { get; private set; }

        public event OnServerShutdownEventDelegate OnServerShutdown;

        public MyEventManager()
        {
            if (Instance == null) Instance = this;
            else MyLogger.LogError("There is already a event manager created. Instance dropped.");
        }

        public void FireShutdownEvent()
        {
            OnServerShutdown?.Invoke();
        }
    }
}