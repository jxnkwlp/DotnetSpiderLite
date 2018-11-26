using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Scheduler.Redis
{
    public interface IRedisStore : IDisposable
    {
        bool RequestExist(string identity);

        void ClearIdentities();

        bool AddRequest(Request request);

        long GetTotalRequestCount();
        long GetCurrentRequestCount();

        Request GetOne();

        long GetSuccessCount();
        long GetErrorCount();

        void IncrementSuccessCount();
        void IncrementErrorCount();

    }
}
