using System;
using System.Collections.Generic;
using System.Text;
using DotnetSpiderLite.Logs;

namespace DotnetSpiderLite.Monitor
{
    public class LogMonitor : IMonitor
    {
        public ILogger Logger { get; set; }

        public void Report(MonitorStatus monitor)
        {
            string msg = $"Left: {monitor.Left}, " +
                $"Success: {monitor.Success}, " +
                $"Error: {monitor.Error}, " +
                $"Total: {monitor.Total}, " +
                $"Dowload: {monitor.AvgDownloadSpeed}, " +
                $"Process: {monitor.AvgProcessorSpeed}, " +
                $"Pipeline: {monitor.AvgPipelineSpeed}";

            Logger?.Trace(msg);
        }
    }
}
