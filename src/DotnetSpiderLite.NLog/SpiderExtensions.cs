using DotnetSpiderLite.NLogs;

namespace DotnetSpiderLite
{
    /// <summary>
    ///  扩展
    /// </summary>
    public static class SpiderExtensions
    {
        /// <summary>
        ///  使用NLog 日志组件
        /// </summary> 
        public static Spider UseNLog(this Spider spider)
        {
            spider.AddLogProvider(new NLogProvider());

            return spider;
        }


    }

}
