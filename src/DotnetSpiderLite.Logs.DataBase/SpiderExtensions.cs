using DotnetSpiderLite.Logs.DataBase;

namespace DotnetSpiderLite
{
    /// <summary>
    ///  扩展
    /// </summary>
    public static class SpiderExtensions
    {
        /// <summary>
        ///  使用数据库存储日志
        /// </summary> 
        public static Spider UseDataBaseLog(this Spider spider, ILoggerWriter loggerWriter)
        {
            spider.AddLogProvider(new LogProvider(loggerWriter));

            return spider;
        }

    }

}
