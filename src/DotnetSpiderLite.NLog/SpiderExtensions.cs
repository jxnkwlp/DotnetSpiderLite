using DotnetSpiderLite.NLogs;

namespace DotnetSpiderLite
{
    public static class SpiderExtensions
    {
        public static Spider UseNLog(this Spider spider)
        {
            spider.SetLogFactory(new NLogLoggerFactory());

            return spider;
        }


    }

}
