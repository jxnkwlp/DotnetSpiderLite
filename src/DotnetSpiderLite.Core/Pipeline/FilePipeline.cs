using DotnetSpiderLite.Abstractions;
using DotnetSpiderLite.Abstractions.Pipeline;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpiderLite.Pipeline
{
    public class FilePipeline : BasePipeline
    {
        public override void Process(IList<ResultItems> resultItems)
        {
            try
            {
                foreach (var resultItem in resultItems)
                {
                    string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "files", $"{Guid.NewGuid():N}.dsd");
                    using (StreamWriter printWriter = new StreamWriter(File.OpenWrite(filePath), Encoding.UTF8))
                    {
                        printWriter.WriteLine("url:\t" + resultItem.Page.Response.Request.Uri);

                        foreach (var entry in resultItem)
                        {
                            if (entry.Value is IList value)
                            {
                                IList list = value;
                                printWriter.WriteLine(entry.Key + ":");
                                foreach (var o in list)
                                {
                                    printWriter.WriteLine(o);
                                }
                                //resultItem.Request.AddCountOfResults(list.Count);
                                //resultItem.Request.AddEffectedRows(list.Count);

                            }
                            else
                            {
                                printWriter.WriteLine(entry.Key + ":\t" + entry.Value);

                                //resultItem.Request.AddCountOfResults(1);
                                //resultItem.Request.AddEffectedRows(1);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger?.Error($"Storage data to file failed: {e}.");
                throw;
            }
        }
    }
}
