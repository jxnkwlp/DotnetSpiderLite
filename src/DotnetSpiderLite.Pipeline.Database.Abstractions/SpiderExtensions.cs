using DotnetSpiderLite.Pipeline.Database;

namespace DotnetSpiderLite.Pipeline
{
    public static class SpiderExtensions
    {
        public static Spider AddDataBasePipeline(this Spider spider, IDatabaseStore databaseStore)
        {
            return spider.AddPipeline(new DatabasePipeline(databaseStore));
        }

    }
}
