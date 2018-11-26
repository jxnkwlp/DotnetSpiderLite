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

        private string _successKey;
        private string _errorKey;
        private string _queueIdentityKey;
        private string _queueKey;


        public RedisStore(string identity, string connectString)
        {
            Connection = ConnectionMultiplexer.Connect(connectString);

            Database = Connection.GetDatabase();

            _successKey = $"dotnetspiderlite:scheduler:{identity}:request:successcount";
            _errorKey = $"dotnetspiderlite:scheduler:{identity}:request:errorcount";
            _queueIdentityKey = $"dotnetspiderlite:scheduler:{identity}:request:queue:identity";
            _queueKey = $"dotnetspiderlite:scheduler:{identity}:request:queue:list";
        }

        public bool RequestExist(string identity)
        {
            return Database.SetContains(_queueIdentityKey, identity);
        }

        public bool AddRequest(Request request)
        {
            Database.SetAdd(_queueIdentityKey, request.GetIdentity());

            var json = request.ToJsonStrng();

            Database.ListLeftPush(_queueKey, json);

            return true;
        }

        public long GetTotalRequestCount()
        {
            return Database.SetLength(_queueIdentityKey);
        }

        public long GetCurrentRequestCount()
        {
            return Database.ListLength(_queueKey);
        }

        public Request GetOne()
        {
            var json = Database.ListLeftPop(_queueKey);

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
            // 清理
            this.Database.KeyDelete(_successKey);
            this.Database.KeyDelete(_errorKey);
            this.Database.KeyDelete(_queueIdentityKey);
            this.Database.KeyDelete(_queueKey);


            // 关键链接
            Connection.Close();
            Connection.Dispose();

        }
    }
}
