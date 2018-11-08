using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Scheduler
{
    /// <summary>
    ///  队列 监控
    /// </summary>
    public interface ISchedulerMonitor
    {
        /// <summary>
        /// 剩余链接数
        /// </summary>
        long LeftRequestsCount { get; }

        /// <summary>
        /// 总的链接数
        /// </summary>
        long TotalRequestsCount { get; }

        /// <summary>
        /// 采集成功的链接数
        /// </summary>
        long SuccessRequestsCount { get; }

        /// <summary>
        /// 采集失败的次数, 不是链接数, 如果一个链接采集多次都失败会记录多次
        /// </summary>
        long ErrorRequestsCount { get; }

        /// <summary>
        /// 采集成功的链接数加 1
        /// </summary>
        void IncreaseSuccessCount();

        /// <summary>
        /// 采集失败的次数加 1
        /// </summary>
        void IncreaseErrorCount();

    }
}
