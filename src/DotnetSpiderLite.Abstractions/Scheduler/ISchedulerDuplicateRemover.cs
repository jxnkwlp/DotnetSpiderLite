using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Scheduler
{
    /// <summary>
    ///  队列去重器
    /// </summary>
    public interface ISchedulerDuplicateRemover : IDisposable
    {
        /// <summary>
        ///  是否重复请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns>true 为重复请求</returns>
        bool IsDuplicate(Request request);

        /// <summary>
        ///  重置去重器
        /// </summary>
        void ResetDuplicateCheck();


    }
}
