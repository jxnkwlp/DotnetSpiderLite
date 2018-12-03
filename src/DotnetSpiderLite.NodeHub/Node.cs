using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.NodeHub
{
    public class Node
    {
        public string NodeId { get; set; }

        public string IpAddress { get; set; }

        public long FreeMemory { get; set; }

        public long TotalMemory { get; set; }

        public string Type { get; set; }

        /// <summary>
        ///  eg: windows/centos...
        /// </summary>
        public string Os { get; set; }

        /// <summary>
        ///  eg: 6.1
        /// </summary>
        public string OsVersion { get; set; }

        public int CpuCoreCount { get; set; }

        public DateTime? LastHeartbeat { get; set; }

    }
}
