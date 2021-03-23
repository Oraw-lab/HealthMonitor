using System;
using System.Diagnostics;
using System.Threading;
namespace ConsoleApp1
{
    class ProcessCPU
    {
        // member object
        private PerformanceCounter processPreformance;
        readonly string typeOfMonitor = "% Processor Time";

        // CTOR - on what process to monitor
        public ProcessCPU(string processName)
        {
            processPreformance = new PerformanceCounter("Process", typeOfMonitor,processName, true);
        }

        // getting current process CPU
        public float getCurrentProcessCPU()
        {
            Thread.Sleep(1000);
            return processPreformance.NextValue();
        }
    }
}
