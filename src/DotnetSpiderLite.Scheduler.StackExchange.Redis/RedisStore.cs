using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Scheduler.Redis
{
    public class RedisStore : IRedisStore
    {
        private ConnectionMultiplexer Connection;
        private IDatabase Database { get; }


        private string _requestkey = "DotnetSpiderLite:Scheduler:RequestCount";
        private string _successKey = "DotnetSpiderLite:Scheduler:SuccessCount";
        private string _errorKey = "DotnetSpiderLite:Scheduler:ErrorCount";
        private string _requestPrefixKey = "DotnetSpiderLite:Scheduler:Request:";


        public RedisStore(string connectString)
        {
            Connection = ConnectionMultiplexer.Connect(connectString);
            Database = Connection.GetDatabase();
        }

        public bool RequestExist(string identity)
        {
            throw new NotImplementedException();
        }

        public bool AddRequest(string identity)
        {
            throw new NotImplementedException();
        }

        public bool AddRequest(Request request)
        {
            throw new NotImplementedException();
        }

        public long GetTotalRequestCount()
        {
            throw new NotImplementedException();
        }

        public long GetCurrentRequestCount()
        {
            throw new NotImplementedException();
        }

        public Request GetOne()
        {
            throw new NotImplementedException();
        }

        public long GetSuccessCount()
        {
            throw new NotImplementedException();
        }

        public long GetErrorCount()
        {
            throw new NotImplementedException();
        }

        public void IncrementSuccessCount()
        {
            throw new NotImplementedException();
        }

        public void IncrementErrorCount()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
