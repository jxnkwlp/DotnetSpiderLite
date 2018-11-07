using DotnetSpiderLite.Abstractions;
using DotnetSpiderLite.Abstractions.Downloader;
using DotnetSpiderLite.Abstractions.Logs;
using DotnetSpiderLite.Abstractions.PageProcessor;
using DotnetSpiderLite.Abstractions.Pipeline;
using DotnetSpiderLite.Abstractions.Scheduler;
using DotnetSpiderLite.Downloader;
using DotnetSpiderLite.Logs;
using DotnetSpiderLite.Pipeline;
using DotnetSpiderLite.Scheduler;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotnetSpiderLite
{
    /// <summary>
    ///  main
    /// </summary>
    public class Spider : IDisposable
    {
        private bool _init = false;
        private IScheduler _scheduler;
        private IDownloader _downloader = new DefaultHttpClientDownloader();
        private SpiderState _spiderState = SpiderState.Init;
        private int _threadNumber = 1;

        public SpiderState State => _spiderState;

        public string Identity { get; set; }


        public int ThreadNumber { get => _threadNumber; set => _threadNumber = value; }


        public ILogger Logger { get; private set; }


        public IList<IPipeline> Pipelines { get; private set; } = new List<IPipeline>();

        public IList<IPageProcessor> PageProcessors { get; private set; } = new List<IPageProcessor>();


        public IScheduler Scheduler { get => _scheduler; set => _scheduler = value; }
        public IDownloader Downloader { get => _downloader; set => _downloader = value; }


        //public IHtmlExtracter PageExtracter { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }


        #region ctor

        protected Spider() : this(null, null, null)
        {
            Init();
        }

        public Spider(string identity, IEnumerable<IPageProcessor> pageProcessors, IEnumerable<IPipeline> pipelines)
        {
            Init();

            this.Identity = identity ?? Guid.NewGuid().ToString();

            if (pageProcessors != null)
                this.PageProcessors = new List<IPageProcessor>(pageProcessors);

            if (pipelines != null)
                this.Pipelines = new List<IPipeline>(pipelines);


        }

        #endregion


        #region Public Methods

        public static Spider Create(Uri uri)
        {
            var spider = new Spider(null, null, null);
            spider.AddRequest(uri);

            return spider;
        }

        public static Spider Create(string url)
        {
            return Create(new Uri(url));
        }

        public void AddRequest(string url, Dictionary<string, string> exts = null)
        {
            this.Scheduler.Enqueue(new Request(new Uri(url)));
        }

        public void AddRequest(Uri uri, Dictionary<string, string> exts = null)
        {
            this.Scheduler.Enqueue(new Request(uri));
        }

        public void AddPipelines(IPipeline pipeline)
        {
            this.Pipelines.Add(pipeline);
        }

        public void AddPageProcessors(IPageProcessor pageProcessor)
        {
            this.PageProcessors.Add(pageProcessor);
        }

        public void AddLog(ILoggerFactory loggerFactory)
        {
            this.Logger = loggerFactory.GetLogger(typeof(Spider));
        }

        public void Run()
        {
            if (_spiderState == SpiderState.Running)
            {
                return;
            }

            _spiderState = SpiderState.Running;

            while (_spiderState == SpiderState.Running || _spiderState == SpiderState.Paused)
            {
                if (_spiderState == SpiderState.Paused)
                {
                    Thread.Sleep(10);
                    continue;
                };

                Parallel.For(0, _threadNumber, new ParallelOptions() { MaxDegreeOfParallelism = _threadNumber },
                    (index) =>
                {

                    while (_spiderState == SpiderState.Running)
                    {
                        // 取出 
                        var request = Scheduler.Dequeue();

                        if (request == null)
                        {
                            _spiderState = SpiderState.Finished;
                            break;
                        }

                        try
                        {
                            RunCore(request).Wait();
                        }
                        catch (Exception)
                        {
                        }
                    }


                    SafeDestroy();

                });


                this.Dispose(true);
            }


        }


        private void Init()
        {
            if (_init) return;
            _init = true;

            if (this.Logger == null)
                this.Logger = new ConsoleLogger();

            WriteInfo();

            if (this.Scheduler == null)
                this.Scheduler = new SampleQueueScheduler();

            if (this.Downloader == null)
                this.Downloader = new DefaultHttpClientDownloader();

            if (this.Pipelines.Count == 0)
                this.Pipelines.Add(new ConsolePipeline());

        }

        private async Task RunCore(Request request)
        {
            // 下载页面 
            var page = await HandleDownloadAsync(request.Uri);

            if (page == null)
                return;

            if (string.IsNullOrEmpty(page.Html))
                return;

            if (page.Skip)
                return;

            if (page.Retry) { }


            // 页面处理程序
            foreach (var processor in PageProcessors)
            {
                try
                {
                    await processor.Process(page);
                }
                catch (Exception ex)
                {
                    this.Logger?.Error(ex.Message);
                }
            }


            if (page.TargetRequests != null && page.TargetRequests.Count > 0)
            {
                foreach (var item in page.TargetRequests)
                {
                    // 添加到 队列 
                    this.Scheduler.Enqueue(item);
                }
            }

            if (page.ResutItems.Count == 0)
                return;

            // 数据处理 
            foreach (var pipeline in Pipelines)
            {
                pipeline.Process(new List<ResultItems> { page.ResutItems });
            }

        }


        private async Task<Page> HandleDownloadAsync(Uri uri)
        {
            try
            {
                var response = await this.Downloader.DownloadAsync(new Request(uri));

                var page = new Page(response);
                //page.HtmlExtracter = this.PageExtracter;

                return page;
            }
            catch (Exception)
            {
            }

            return null;
        }


        private void SafeDestroy()
        {

        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~Spider() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        void IDisposable.Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion


        //internal void SendExitSignal()
        //{
        //    if (Env.IsWindows)
        //    {
        //        var identityMmf = MemoryMappedFile.OpenExisting(Identity, MemoryMappedFileRights.Write);
        //        using (MemoryMappedViewStream stream = identityMmf.CreateViewStream())
        //        {
        //            var writer = new BinaryWriter(stream);
        //            writer.Write(1);
        //        }

        //        try
        //        {
        //            var taskIdMmf = MemoryMappedFile.OpenExisting(TaskId, MemoryMappedFileRights.Write);
        //            using (MemoryMappedViewStream stream = taskIdMmf.CreateViewStream())
        //            {
        //                var writer = new BinaryWriter(stream);
        //                writer.Write(1);
        //            }
        //        }
        //        catch
        //        {
        //            //ignore
        //        }
        //    }
        //    else
        //    {
        //        File.Create(_filecloseSignals[0]);
        //    }
        //}

        #endregion


        void WriteInfo()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("================================================");
            sb.AppendLine("== Dotnet Spider Lite An open source crawler  ==");
            sb.AppendLine("== Power by C#                                ==");
            sb.AppendLine("== Author : jxnkwlp                           ==");
            sb.AppendLine("================================================");

            this.Logger?.Info(sb.ToString());
        }

    }
}
