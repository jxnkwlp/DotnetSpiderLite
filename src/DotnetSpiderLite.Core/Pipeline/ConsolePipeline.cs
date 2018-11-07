using DotnetSpiderLite.Abstractions;
using DotnetSpiderLite.Abstractions.Logs;
using DotnetSpiderLite.Abstractions.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpiderLite.Pipeline
{
    public class ConsolePipeline : BasePipeline
    {
        public override void Process(IList<ResultItems> resultItems)
        {
            foreach (var resultItem in resultItems)
            {
                foreach (var entry in resultItem)
                {
                    System.Console.WriteLine(entry.Key + ":\t" + entry.Value);

                }
            }
        }
    }
}
