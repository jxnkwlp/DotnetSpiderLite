using DotnetSpiderLite;
using DotnetSpiderLite.Pipeline;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpiderLite.Pipeline
{
    public class FilePipeline : BaseFilePipeline
    {
        public FilePipeline() : base("")
        {
        }

        public FilePipeline(string target) : base(target)
        {
        }

        public override void Process(IList<ResultItems> resultItems)
        {
            try
            {
                string filePath = Path.Combine(DataFolder, "files", DateTime.Now.ToString("yyyyMMdd"), $"{Guid.NewGuid():N}.dsd");

                if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                }


                foreach (var resultItem in resultItems)
                {
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

                            }
                            else
                            {
                                printWriter.WriteLine(entry.Key + ":\t" + entry.Value);

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
