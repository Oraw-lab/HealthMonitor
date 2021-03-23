using System;
using System.Collections.Generic;
using System.Threading;
namespace ConsoleApp1
{
    class Program
    {
        static void Main(string [] args)
        {
            var threadObject = new ThreadManger();
            List<Thread> listOfThreads = new List<Thread>();
            foreach(var process in args)
            {
                var monitorObject = new MonitorProcess(process);
                listOfThreads.Add(new Thread(() => monitorObject.startMonitorProcess(threadObject.getBool())));
            }
            threadObject.setThreadList(listOfThreads);

        }
    }
}
