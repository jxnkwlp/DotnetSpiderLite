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
        /// <param name="spider"></param>
        /// <param name="redisConnectionString">redis链接字符串</param>
        /// <returns></returns>
        public static Spider UseRedisScheduler(this Spider spider, string redisConnectionString)
        {
            var identity = spider.Identity;
            spider.UseRedisScheduler(new RedisStore(identity, redisConnectionString));
            return spider;
        }
    }
}
