using DotnetSpiderLite.Scheduler.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Scheduler
{
    /// <summary>
    ///  扩展
    /// </summary>
    public static class SpiderExtensions
    {
        /// <summary>
        ///  使用 redis 作为队列
        /// </summary>
        /// <typeparam name="TRedisStore"></typeparam>
        /// <param name="spider"></param>
        /// <param name="redisStore">指定 redis 存储服务</param>
        /// <returns></returns>
        public static Spider UseRedisScheduler<TRedisStore>(this Spider spider, TRedisStore redisStore) where TRedisStore : IRedisStore
        {
            spider.SetScheduler(new RedisScheduler(redisStore));
            return spider;
        }
    }
}
