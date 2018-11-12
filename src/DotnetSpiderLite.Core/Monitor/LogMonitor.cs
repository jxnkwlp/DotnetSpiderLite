using System;
using System.Collections.Generic;
using System.Text;
using DotnetSpiderLite.Logs;

namespace DotnetSpiderLite.Monitor
{
    public class LogMonitor : IMonitor
    {
        public ILogger Logger { get; set; }

        public void Report(MonitorData monitor)
        {
            string msg = $"Left: {monitor.Left}, " +
                $"Success: {monitor.Success}, " +
                $"Error: {monitor.Error}, " +
                $"Total: {monitor.Total}, " +
                $"Dowload: {monitor.AvgDownloadSpeed} ms/per, " +
                $"Process: {monitor.AvgProcessorSpeed} ms/per, " +
                $"Pipeline: {monitor.AvgPipelineSpeed} ms/per";

            Logger?.Trace(msg);
        }
    }
}
