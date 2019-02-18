using DotnetSpiderLite.Log4net;

namespace DotnetSpiderLite
{
    /// <summary>
    ///  扩展
    /// </summary>
    public static class SpiderExtensions
    {
        /// <summary>
        ///  使用 Log4net 日志组件
        /// </summary> 
        public static Spider UseLog4net(this Spider spider)
        {
            spider.SetLogFactory(new Log4LoggerFactory());

            return spider;
        }


    }

}
