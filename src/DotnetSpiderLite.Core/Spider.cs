using DotnetSpiderLite.Downloader;
using DotnetSpiderLite.Html;
using DotnetSpiderLite.Logs;
using DotnetSpiderLite.PageProcessor;
using DotnetSpiderLite.Pipeline;
using DotnetSpiderLite.Scheduler;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace DotnetSpiderLite
{
    /// <summary>
    ///  main
    /// </summary>
    public class Spider : IDisposable, IIdentity
    {
        private bool disposedValue = false; // 要检测冗余调用
        private int _resultItemsCacheCount = 10; // 默认10
        private bool _init = false;
        private IScheduler _scheduler;
        private IDownloader _downloader = new DefaultHttpClientDownloader();
        private SpiderStatus _spiderStatus = SpiderStatus.Init;
        private int _threadNumber = 1;
        private int _exitWaitInterval = 10 * 1000;
        private int _waitCountLimit = 1000;

        private ConcurrentBag<ResultItems> _cacheResultItems = new ConcurrentBag<ResultItems>();


        /// <summary> 
        ///  sleep time before request url. default 0.1s
        /// </summary>
        public int SleepTime { get; set; } = 100;

        /// <summary>
        ///  sleep time when requests is empty . default 15s
        /// </summary>
        public int EmptySleepTime { get; set; } = 15 * 1000;


        public SpiderStatus Status => _spiderStatus;

        public string Identity { get; set; }

        public int ThreadNumber { get => _threadNumber; set => _threadNumber = value; }

        public ILogger Logger { get; private set; }

        public IList<IPipeline> Pipelines { get; private set; } = new List<IPipeline>();

        public IList<IPageProcessor> PageProcessors { get; private set; } = new List<IPageProcessor>();

        public IScheduler Scheduler { get => _scheduler; set => _scheduler = value; }

        public IDownloader Downloader { get => _downloader; set => _downloader = value; }


        public IHtmlElementSelectorFactory SelectorFactory { get; private set; }



        public int ResultItemsCacheCount { get => _resultItemsCacheCount; set => _resultItemsCacheCount = value; }



        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }



        public event Action<Spider, bool> OnClosed;



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

        public Spider AddRequest(string url, Dictionary<string, string> exts = null)
        {
            this.Scheduler.Push(new Request(new Uri(url)));
            return this;
        }

        public Spider AddRequest(Uri uri, Dictionary<string, string> exts = null)
        {
            this.Scheduler.Push(new Request(uri));
            return this;
        }

        public Spider AddPipelines(IPipeline pipeline)
        {
            this.Pipelines.Add(pipeline);
            return this;
        }

        public Spider AddPageProcessors(IPageProcessor pageProcessor)
        {
            this.PageProcessors.Add(pageProcessor);
            return this;
        }

        public Spider SetLogFactory(ILoggerFactory loggerFactory)
        {
            this.Logger = loggerFactory.GetLogger(typeof(Spider));
            return this;
        }

        public Spider SetScheduler(IScheduler scheduler)
        {
            this.Scheduler = scheduler;
            return this;
        }

        public Spider SetDownloader(IDownloader downloader)
        {
            this.Downloader = downloader;
            return this;
        }

        public Spider SetMaxThreadNumber(int value)
        {
            this.ThreadNumber = value;
            return this;
        }




        public void Run()
        {
            if (_spiderStatus == SpiderStatus.Running)
            {
                return;
            }

            _spiderStatus = SpiderStatus.Running;

            while (_spiderStatus == SpiderStatus.Running || _spiderStatus == SpiderStatus.Paused)
            {
                if (_spiderStatus == SpiderStatus.Paused)
                {
                    Thread.Sleep(10);
                    continue;
                };

                Parallel.For(0, _threadNumber, new ParallelOptions() { MaxDegreeOfParallelism = _threadNumber },
                    (index) =>
                {
                    int waitCount = 1;

                    while (_spiderStatus == SpiderStatus.Running)
                    {
                        // 取出 
                        var request = Scheduler.Pull();

                        if (request == null)
                        {
                            // 等待超时，退出
                            if (waitCount > _waitCountLimit)
                            {
                                _spiderStatus = SpiderStatus.Finished;
                                break;
                            }

                            // 等待
                            WaitNewRequest(ref waitCount);

                        }
                        else
                        {
                            waitCount = 1;

                            try
                            {
                                var downloader = this.Downloader.Clone();

                                // 
                                HandleRequestAsync(request, downloader).Wait();

                                Thread.Sleep(SleepTime);
                            }
                            catch
                            {
                            }

                        }
                    }

                });


            }

            if (_spiderStatus == SpiderStatus.Finished)
            {
                _spiderStatus = SpiderStatus.Exited;

                this.Closed();
            }
        }


        public void Contiune()
        {
            if (_spiderStatus != SpiderStatus.Paused)
            {
                Logger.Warn("Current status not paused.");
            }
            else
            {
                _spiderStatus = SpiderStatus.Paused;
                Logger.Info("Spider contiune run.");
            }
        }

        public void Pause()
        {
            if (_spiderStatus != SpiderStatus.Running)
            {
                Logger.Warn("Current status not running.");
            }
            else
            {
                _spiderStatus = SpiderStatus.Paused;
                Logger.Info("Spider Paused.");
            }
        }



        private void WaitNewRequest(ref int count)
        {
            Thread.Sleep(10);
            ++count;
        }








        private void Init()
        {
            if (_init) return;
            _init = true;

            if (this.Logger == null)
                this.Logger = LogManager.GetLogger(typeof(Spider));

            WelcomeInfo();

            InitComponents();

        }

        private void InitComponents()
        {
            if (this.Scheduler == null)
                this.Scheduler = new SampleQueueScheduler() { Logger = this.Logger };

            if (this.Downloader == null)
                this.Downloader = new DefaultHttpClientDownloader() { Logger = this.Logger };

            if (this.Pipelines.Count == 0)
                this.Pipelines.Add(new ConsolePipeline() { Logger = this.Logger });


            InitHtmlQuery();
        }

        private void InitHtmlQuery()
        {
            // TODO: 更优雅的方法 ???  

            if (this.SelectorFactory != null)
                return;

            TryLoadHtmlAgilityPackHtmlSelectorFactory();

        }

        private void TryLoadHtmlAgilityPackHtmlSelectorFactory()
        {
            try
            {
                var ass = Assembly.Load("DotnetSpiderLite.HtmlAgilityPack");
                var type = ass.GetType("DotnetSpiderLite.HtmlAgilityPack.HtmlElementSelectorFactory");

                var typeObj = Activator.CreateInstance(type);

                this.SelectorFactory = typeObj as IHtmlElementSelectorFactory;
            }
            catch (Exception)
            {
                this.Logger.Info("Can't load HtmlAgilityPack.HtmlElementSelectorFactory ");
            }
        }

        private async Task HandleRequestAsync(Request request, IDownloader downloader)
        {
            // 下载页面 
            var page = await HandleDownloadAsync(request, downloader);

            if (page == null)
                return;

            if (string.IsNullOrEmpty(page.Html))
                return;

            if (page.Skip)
                return;

            if (page.Retry)
            {
                // 添加到 队列 
                this.Scheduler.Push(request);
                return;
            }


            // 页面处理程序
            HandlePageProcessors(page);


            if (page.TargetRequests != null && page.TargetRequests.Count > 0)
            {
                foreach (var item in page.TargetRequests)
                {
                    // 添加到 队列 
                    this.Scheduler.Push(item);
                }
            }

            if (page.ResutItems.Count == 0)
                return;

            // 数据处理 
            HandlePipelines(page);
        }


        private void HandlePageProcessors(Page page)
        {
            foreach (var processor in PageProcessors)
            {
                try
                {
                    processor.Process(page);
                }
                catch (Exception ex)
                {
                    this.Logger?.Error($"The processor of '{processor.GetType().FullName}' Process faild.");
                    this.Logger?.Error(ex.Message);
                }
            }
        }


        private void HandlePipelines(Page page)
        {
            if (this._resultItemsCacheCount > 0)
            {
                if (_cacheResultItems.Count < this._resultItemsCacheCount)
                {
                    _cacheResultItems.Add(page.ResutItems);
                    return;
                }
            }
            else
            {
                _cacheResultItems.Add(page.ResutItems);

            }


            foreach (var pipeline in Pipelines)
            {
                try
                {
                    pipeline.Process(_cacheResultItems.ToArray());
                }
                catch (Exception ex)
                {
                    this.Logger?.Error($"The pipeline of '{pipeline.GetType().FullName}' Process faild");
                    this.Logger?.Error(ex.Message);
                }

            }

        }


        private async Task<Page> HandleDownloadAsync(Request request, IDownloader downloader)
        {
            var uri = request.Uri;
            this.Logger?.Trace($"Start download url: {uri} ");

            try
            {
                var response = await downloader.DownloadAsync(new Request(uri));

                var page = new Page(response);
                page.Extra.Marge(request.Extra);
                page.Extra["Downloader"] = downloader.GetType().FullName;
                page.Extra["Identity"] = this.Identity;

                if (SelectorFactory != null)
                {
                    page.SetSelector(SelectorFactory.GetSelector(page.Html));
                    page.Extra["Selector"] = page.Selector.GetType().FullName;
                }

                return page;
            }
            catch (Exception ex)
            {
                this.Logger?.Error("Handle download url faild.");
                this.Logger?.Error($"URL:{uri}");
                this.Logger?.Error("Message", ex);
            }

            return null;
        }

        private void Closed()
        {
            this.OnClosed?.Invoke(this, true);
            this.Logger?.Info("Spider exit.");

            SafeDestroy();
        }

        private void SafeDestroy()
        {
            foreach (var item in this.Pipelines)
            {
                item.Dispose();
            }

            foreach (var item in this.PageProcessors)
            {
                item.Dispose();
            }

            this.Downloader.Dispose();
            this.Scheduler.Dispose();


        }



        #region IDisposable Support


        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Closed();
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


        void WelcomeInfo()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("================================================");
            sb.AppendLine("==                                            ==");
            sb.AppendLine("== Welcome to Dotnet Spider Lite              ==");
            sb.AppendLine("== Dotnet Spider Lite An open source crawler  ==");
            sb.AppendLine("==                                            ==");
            sb.AppendLine("================================================");

            Console.WriteLine(sb.ToString());

            this.Logger?.Info("Spider starting... ");
        }

    }
}
