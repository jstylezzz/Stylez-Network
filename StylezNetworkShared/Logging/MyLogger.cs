using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StylezNetworkShared.Logging
{
    public static class MyLogger
    {
        public static void LogInfo(string s)
        {
            DateTime n = DateTime.Now;
            Console.WriteLine($"[{n.Hour}:{n.Minute}:{n.Second}][INFO]: {s}");
        }

        public static void LogError(string s)
        {
            DateTime n = DateTime.Now;
            Console.WriteLine($"[{n.Hour}:{n.Minute}:{n.Second}][ERROR]: {s}");
        }

        public static void LogWarning(string s)
        {
            DateTime n = DateTime.Now;
            Console.WriteLine($"[{n.Hour}:{n.Minute}:{n.Second}][WARNING]: {s}");
        }
    }
}
