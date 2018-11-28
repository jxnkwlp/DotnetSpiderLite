using DotnetSpiderLite.Pipeline.Database;
using DotnetSpiderLite.Pipeline.Database.Dapper;

namespace DotnetSpiderLite.Pipeline
{
    public static class SpiderExtensions
    {
        public static Spider AddDapperDataBasePipeline(this Spider spider, DapperDatabaseStore store)
        {
            return spider.AddDataBasePipeline(store);
        }

    }
}
