using DotnetSpiderLite.Processor.Config;

namespace DotnetSpiderLite
{
    /// <summary>
    ///  扩展
    /// </summary>
    public static class SpiderExtensions
    {
        /// <summary> 
        /// </summary> 
        public static Spider AddProcessorConfig(this Spider spider, IConfigResolve resolve)
        {

            return spider;
        }


    }

}
