using System.Collections.Generic;
using System.Threading;

namespace ConsoleApp1
{
    class ThreadManger
    {
        // object mebers
        private bool programRunning = true;
        private List<Thread> allThreads;
        
        // public methods

        // returning if program running
        public bool getBool()
        {
            return programRunning;
        }

        // setting all threads to join
        public void setThreadList(List<Thread> _allThreads)
        {
            allThreads = _allThreads;
            StartAllThreads();
        }

        // Gurading the bool of program running
        public class GuradBool
        {
            // object member
            private bool programRunning { get; set; }

            // On CTOR setting prgoram running
            // getting ThreadManger since bool can only be copied
            public GuradBool(ThreadManger threadManger)
            {
                this.programRunning = threadManger.getBool();
                this.programRunning = true;
            }

            // once getting out of scope program stops running
            ~GuradBool()
            {
                this.programRunning = false;
            }
            
        }


        // starting all thread in list
        public void StartAllThreads()
        {
            foreach (var thread in allThreads)
            {
                thread.Start();
            }
            foreach (var thread in allThreads)
            {
                thread.Join();
            }

        }

    }
}
