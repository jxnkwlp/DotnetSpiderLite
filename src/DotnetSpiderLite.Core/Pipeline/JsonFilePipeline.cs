using DotnetSpiderLite;
using DotnetSpiderLite.Pipeline;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DotnetSpiderLite.Pipeline
{
    public class JsonFilePipeline : BaseFilePipeline
    {
        private readonly ConcurrentDictionary<string, StreamWriter> _writers = new ConcurrentDictionary<string, StreamWriter>();


        public JsonFilePipeline() : base("")
        {

        }

        public JsonFilePipeline(string target) : base(target)
        {
        }

        public override void Process(IList<ResultItems> resultItems)
        {
            string filePath = Path.Combine(DataFolder, "files", DateTime.Now.ToString("yyyyMMdd"), $"{Guid.NewGuid():N}.json");

            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                }

                var streamWriter = GetStreamWriter(filePath);

                foreach (var resultItem in resultItems)
                {
                    streamWriter.WriteLine(JsonConvert.SerializeObject(resultItem));
                }
            }
            catch (Exception e)
            {
                Logger?.Error($"Storage data to file {filePath} failed: {e}.");
                throw;
            }
        }

        private StreamWriter GetStreamWriter(string file)
        {
            if (_writers.ContainsKey(file))
            {
                return _writers[file];
            }
            else
            {
                var streamWriter = new StreamWriter(File.OpenWrite(file), Encoding.UTF8);
                _writers.TryAdd(file, streamWriter);
                return streamWriter;
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            foreach (var pair in _writers)
            {
                pair.Value.Dispose();
            }

            _writers.Clear();
        }
    }
}
