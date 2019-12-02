using System;
using System.Diagnostics;

namespace Ophelia.Net
{
    public class NetworkAdapter
    {
        internal NetworkAdapter(string name)
        {
            this.sName = name;
        }

        private long nDownloadSpeed, nUploadSpeed;
        private long nDownloadValue, nUploadValue;
        private long nOldDownloadValue, nOldUploadValue;

        internal string sName;
        internal PerformanceCounter oDownloadCounter, oUploadCounter;

        public string Name
        {
            get
            {
                return this.sName;
            }
        }
        public long DownloadSpeed
        {
            get
            {
                return this.nDownloadSpeed;
            }
        }
        public long UploadSpeed
        {
            get
            {
                return this.nUploadSpeed;
            }
        }
        public double DownloadSpeedKbps
        {
            get
            {
                return this.nDownloadSpeed / 1024.0;
            }
        }
        public double UploadSpeedKbps
        {
            get
            {
                return this.nUploadSpeed / 1024.0;
            }
        }
        internal void Init()
        {
            this.nOldDownloadValue = this.oDownloadCounter.NextSample().RawValue;
            this.nOldUploadValue = this.oUploadCounter.NextSample().RawValue;
        }
        internal void Refresh()
        {
            this.nDownloadValue = this.oDownloadCounter.NextSample().RawValue;
            this.nUploadValue = this.oUploadCounter.NextSample().RawValue;

            this.nDownloadSpeed = this.nDownloadValue - this.nOldDownloadValue;
            this.nUploadSpeed = this.nUploadValue - this.nOldUploadValue;

            this.nOldDownloadValue = this.nDownloadValue;
            this.nOldUploadValue = this.nUploadValue;
        }
        public override string ToString()
        {
            return this.sName;
        }
    }
}
