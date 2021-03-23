using System;
using System.Collections.Generic;
using System.Threading;

namespace ConsoleApp1
{
    class MonitorProcess
    {
        // object variable
        // monitoring process
        readonly private string processName;
        // All spikes are stored here
        private List<bool> spikesOfProcess;
        // thresholds
        private int continuosSpikes;
        private int defaultSpikes;

        // Threshold for process we run on
        private float processThreshold;

        // thread for collection
        private Thread thread = null;
        // validation that finsihed running
        private bool isBeingCollected = false;

        private class BoolWrapper
        {
            // class memeber
            private bool isBeingCollected { get; set; }
            // CTOR setting data collection started
            public BoolWrapper(MonitorProcess value) { this.isBeingCollected = value.getBool(); this.isBeingCollected = true; }
            // DTOR setting data collection finished
            ~BoolWrapper()
            {
                isBeingCollected = false;
            }
        }

        // Public methods 

        // getting is data being collected
        public bool getBool()
        {
            return isBeingCollected;
        }

        // CTOR - string processName - on what process to monitor 
        //default thresholds will be set
        public MonitorProcess(string _processName)
        {
            processName = _processName;
            List<bool> tempList = new List<bool>(120);
            spikesOfProcess = tempList;
            defaultSpikes = 120 / 2;
            continuosSpikes = 120 / 4;
        }
        
        // CTOR - string processName - on what process to monitor
        // rest of ints - thresholds to set
        public MonitorProcess(string _processName,int _continuosSpikes,int _defaultSpikes, int _processThreadhold, int timePreCycle)
        {
            if(_continuosSpikes == 0 || _defaultSpikes == 0 || _processThreadhold == 0 || timePreCycle == 0)
            {
                // not allowed to be 0
                throw new Exception("threadhold cant be 0");
            }
            var tempList = new List<bool>(timePreCycle);
            spikesOfProcess = tempList;
            processName = _processName;
            continuosSpikes = _continuosSpikes;
            defaultSpikes = _defaultSpikes;
            processThreshold = _processThreadhold;
        }

        // Main thread to run on 
        // Monitoring the process that was set in CTOR
        public void startMonitorProcess(bool programRunning)
        {
            // creating object
            var checkProcessRunning = new CheckProcessAlive(processName);
            var processCPU = new ProcessCPU(processName);
            int placeInVector = 0;
            int sizeOfVector = spikesOfProcess.Count;
            int continuosSpikesCounter = 0;
            int defaultSpikesCounter = 0;
            while (programRunning)
            {
                if(checkProcessRunning.getProcessRunning())
                {
                    // getting process spike
                    var currentProcessCPU = processCPU.getCurrentProcessCPU();
                    // deciding what to do with current CPU
                    var currentRes = handleCPUSpike(currentProcessCPU > processThreshold, placeInVector);
                    placeInVector++;
                    switch(currentRes)
                    {
                        // was a spike but not there isnt
                        case -1:
                            defaultSpikesCounter--;
                            continuosSpikesCounter = 0;
                            break;
                        // was a spike / wasnt a spike but the same result was on prev time
                        case 0:
                            if(currentProcessCPU > processThreshold)
                            {
                                // found a spike to make sure continousCounter is fine
                                continuosSpikesCounter++;
                            }
                            else
                            {
                                // no spike found reseting
                                continuosSpikesCounter = 0;
                            }
                            break;
                        case 1:
                            // there wasnt a spike but now there is
                            defaultSpikesCounter++;
                            continuosSpikesCounter++;
                            break;
                    }
                    // if found enough spikes starting to collect
                    if(defaultSpikes == defaultSpikesCounter || continuosSpikes == continuosSpikesCounter)
                    {
                        // enough spikes collecting data
                        thread = new Thread(collectData);
                        thread.Start();
                    }
                    if(!thread.IsAlive && !isBeingCollected)
                    {
                        thread.Join();
                        thread = null;
                    }
                }
                else
                {
                    // if process not alive waiting for it to be alive
                    Thread.Sleep(10000);
                    // Nulling all variable since process probably restarted
                    spikesOfProcess.RemoveAll(item => item == false);
                    continuosSpikesCounter = 0; defaultSpikesCounter = 0;
                }
            }
        }

        private void collectData()
        {
            var g = new BoolWrapper(this);
            // DO WORK
        }

        private int handleCPUSpike(bool isSpike, int currentPlaceIn)
        {
            var prevPosSpike = spikesOfProcess[currentPlaceIn];
            if (isSpike)
            {
                spikesOfProcess[currentPlaceIn] = true;
            }
            else
            {
                spikesOfProcess[currentPlaceIn] = false;
            }

            if (prevPosSpike && isSpike)
            {
                // had a spike and also spiking now
                return 0;
            }
            else if (prevPosSpike && !isSpike)
            {
                // was spiking but now not
                return -1;
            }
            else if (!prevPosSpike && isSpike)
            {
                // was not spiking but now spiking
                return 1;
            }
            else if (!prevPosSpike && !isSpike)
            {
                // not spiking at all
                return 0;
            }
            return 0;
        }
    }
}
