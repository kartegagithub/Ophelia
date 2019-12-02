using System;
using System.Collections;
using System.Diagnostics;
using System.Timers;

namespace Ophelia.Net
{
    public class NetworkMonitor
    {
        public NetworkMonitor(long interval)
            : base()
        {
            this.nTimerInterval = interval;
        }
        public NetworkMonitor()
        {
            this.oAdapters = new ArrayList();
            this.oMonitoredAdapters = new ArrayList();
            EnumerateNetworkAdapters();

            oTimer = new Timer(nTimerInterval);
            oTimer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
        }

        private Timer oTimer;
        private long nTimerInterval = 1000;
        private ArrayList oAdapters;
        private ArrayList oMonitoredAdapters;

        public NetworkAdapter[] Adapters
        {
            get
            {
                return (NetworkAdapter[])this.oAdapters.ToArray(typeof(NetworkAdapter));
            }
        }

        private void EnumerateNetworkAdapters()
        {
            PerformanceCounterCategory category = new PerformanceCounterCategory("Network Interface");

            foreach (string name in category.GetInstanceNames())
            {
                if (name == "MS TCP Loopback interface")
                    continue;

                NetworkAdapter adapter = new NetworkAdapter(name);
                adapter.oDownloadCounter = new PerformanceCounter("Network Interface", "Bytes Received/sec", name);
                adapter.oUploadCounter = new PerformanceCounter("Network Interface", "Bytes Sent/sec", name);
                this.oAdapters.Add(adapter);
            }
        }

        public void StartMonitoring()
        {
            if (this.oAdapters.Count > 0)
            {
                foreach (NetworkAdapter adapter in this.oAdapters)
                    if (!this.oMonitoredAdapters.Contains(adapter))
                    {
                        this.oMonitoredAdapters.Add(adapter);
                        adapter.Init();
                    }

                oTimer.Enabled = true;
            }
        }

        public void StartMonitoring(NetworkAdapter adapter)
        {
            if (!this.oMonitoredAdapters.Contains(adapter))
            {
                this.oMonitoredAdapters.Add(adapter);
                adapter.Init();
            }
            oTimer.Enabled = true;
        }

        public void StopMonitoring()
        {
            this.oMonitoredAdapters.Clear();
            oTimer.Enabled = false;
        }

        public void StopMonitoring(NetworkAdapter adapter)
        {
            if (this.oMonitoredAdapters.Contains(adapter))
                this.oMonitoredAdapters.Remove(adapter);
            if (this.oMonitoredAdapters.Count == 0)
                oTimer.Enabled = false;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            foreach (NetworkAdapter adapter in this.oMonitoredAdapters)
                adapter.Refresh();
        }
    }
}
