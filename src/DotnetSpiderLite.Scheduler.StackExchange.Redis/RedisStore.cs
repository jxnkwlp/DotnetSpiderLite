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


        //private string _requestkey = "DotnetSpiderLite:Scheduler:RequestCount";
        private string _successKey = "DotnetSpiderLite:Scheduler:Request:SuccessCount";
        private string _errorKey = "DotnetSpiderLite:Scheduler:Request:ErrorCount";
        private string _requestIdentityKey = "DotnetSpiderLite:Scheduler:Request:Identity";
        private string _requestListKey = "DotnetSpiderLite:Scheduler:Request:List";


        public RedisStore(string connectString)
        {
            Connection = ConnectionMultiplexer.Connect(connectString);

            Database = Connection.GetDatabase();
        }

        public bool RequestExist(string identity)
        {
            return Database.SetContains(_requestIdentityKey, identity);
        }

        public bool AddRequest(Request request)
        {
            Database.SetAdd(_requestIdentityKey, request.GetIdentity());

            var json = request.ToJsonStrng();

            Database.ListLeftPush(_requestListKey, json);

            return true;
        }

        public long GetTotalRequestCount()
        {
            return Database.SetLength(_requestIdentityKey);
        }

        public long GetCurrentRequestCount()
        {
            return Database.ListLength(_requestListKey);
        }

        public Request GetOne()
        {
            var json = Database.ListLeftPop(_requestListKey);

            if (string.IsNullOrEmpty(json))
                return null;

            return json.ToString().ToRequest();
        }

        public long GetSuccessCount()
        {
            string value = Database.StringGet(_successKey);
            if (long.TryParse(value, out long result))
                return result;
            return 0;
        }

        public long GetErrorCount()
        {
            if (Database.StringGet(_errorKey).TryParse(out long result))
                return result;
            return 0;
        }

        public void IncrementSuccessCount()
        {
            Database.StringIncrement(_successKey);
        }

        public void IncrementErrorCount()
        {
            Database.StringIncrement(_errorKey);
        }

        public void Dispose()
        {
            //Connection.Close();
            //Connection.Dispose();
        }
    }
}
