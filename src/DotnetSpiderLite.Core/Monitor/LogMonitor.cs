using System;
using System.Collections.Generic;
using System.Text;
using DotnetSpiderLite.Logs;

namespace DotnetSpiderLite.Monitor
{
    public class LogMonitor : IMonitor
    {
        public ILogger Logger { get; set; }

        public void Report(MonitorData data)
        {
            string msg = $"Left: {data.Left}, " +
                $"Success: {data.Success}, " +
                $"Error: {data.Error}, " +
                $"Total: {data.Total}, " +
                $"Dowload: {data.AvgDownloadSpeed} ms/per, " +
                $"Process: {data.AvgProcessorSpeed} ms/per, " +
                $"Pipeline: {data.AvgPipelineSpeed} ms/per";

            Logger?.Trace(msg);
        }
    }
}
