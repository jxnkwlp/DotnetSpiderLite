using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpiderLite.NLogs
{
    public static class SpiderExtensions
    {
        public static Spider UseNLog(this Spider spider)
        {
            spider.AddLog(new NLogLoggerFactory());

            return spider;
        }


    }

}
