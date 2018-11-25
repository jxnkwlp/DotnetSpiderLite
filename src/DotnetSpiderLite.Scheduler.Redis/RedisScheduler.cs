using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using DotnetSpiderLite.Logs;

namespace DotnetSpiderLite.Scheduler.Redis
{
    public class RedisScheduler : IScheduler, ISchedulerMonitor
    {
        private IRedisStore _redisStore;

        private readonly object _lock = new object();
         
        public ILogger Logger { get; set; }


        public RedisScheduler(IRedisStore redisStore)
        {
            _redisStore = redisStore;
        }


        public long LeftRequestsCount
        {
            get
            {
                lock (_lock)
                {
                    return _redisStore.GetCurrentRequestCount();
                }
            }
        }

        public long TotalRequestsCount => _redisStore.GetTotalRequestCount();

        public long SuccessRequestsCount => _redisStore.GetSuccessCount();

        public long ErrorRequestsCount => _redisStore.GetErrorCount();

        public Request Pull()
        {
            return _redisStore.GetOne();
        }

        public void Dispose()
        {
            _redisStore.Dispose();
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Push(Request request)
        {
            var requestIdentity = request.GetIdentity();

            if (_redisStore.RequestExist(requestIdentity))
            {
                return;
            }
             
            _redisStore.AddRequest(request);
        }

        public void IncreaseSuccessCount()
        {
            _redisStore.IncrementSuccessCount();
        }

        public void IncreaseErrorCount()
        {
            _redisStore.IncrementErrorCount();
        }

    }
}
