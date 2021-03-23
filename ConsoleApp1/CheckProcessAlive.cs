using System;
using System.Diagnostics;

namespace ConsoleApp1
{
    class CheckProcessAlive
    {
        // Object members
        private string processToCheck;

        // CTOR - what process name we should monitor
        public CheckProcessAlive(string _processToCheck)
        {
            processToCheck = _processToCheck;
        }

        // Checking if process still running
        public bool getProcessRunning()
        {
            Process[] pname = Process.GetProcessesByName(processToCheck);
            // if 0 process not running
            return pname.Length == 0;
        }
    }
}
