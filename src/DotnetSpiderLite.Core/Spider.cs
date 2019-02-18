using DotnetSpiderLite.Downloader;
using DotnetSpiderLite.Html;
using DotnetSpiderLite.Logs;
using DotnetSpiderLite.Monitor;
using DotnetSpiderLite.PageProcessor;
using DotnetSpiderLite.Pipeline;
using DotnetSpiderLite.Scheduler;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotnetSpiderLite
{
    /// <summary>
    ///  爬虫 
    /// </summary>
    public class Spider : IDisposable, IIdentity
    {
        private bool disposedValue = false; // 要检测冗余调用
        private int _resultItemsCacheCount = 10; // default 10
        private bool _init = false;
        private int _threadNumber = 1;
        private int _waitCountLimit = 100 * 10;  // 100 是一秒
        private int _exitWaitInterval = 15 * 1000;
        private int _defaultExitWaitInterval = 15 * 1000;

        private List<ResultItems> _cacheResultItems = new List<ResultItems>();

        private long _downloaderCounts = 0;  // 次数
        private long _downloaderCostTimes = 0; // 总时间
        private long _downloadAvgSpeed = 0; // 速度

        private long _processorCounts = 0;  // 次数
        private long _processorCostTimes = 0; // 总时间
        private long _processorAvgSpeed = 0; // 速度

        private long _pipelineCounts = 0;  // 次数
        private long _pipelineCostTimes = 0; // 总时间
        private long _pipelineAvgSpeed = 0; // 速度

        private ILoggerFactory _loggerFactory = new LoggerFactory();
        private IScheduler _scheduler;
        private IDownloader _downloader;
        private SpiderStatus _spiderStatus = SpiderStatus.Init;

        private IList<IDownloadBeforeHandle> _downloadBeforeHandles = new List<IDownloadBeforeHandle>();
        private IList<IDownloadAfterHandle> _downloadAfterHandles = new List<IDownloadAfterHandle>();
        private IList<Request> _initRequest = new List<Request>();
        private Thread _monitorThread;

        private IDownloaderProxy _downloaderProxy;

        private bool _useHttpClientDownloader = true;
        private int _newRequestSleepInterval = 100;
        private readonly object _lockObject = new object();

        /// <summary> 
        ///  新请求前暂停时间(毫秒)，默认 100ms
        /// </summary>
        public int NewRequestSleepInterval
        {
            get => _newRequestSleepInterval;
            set
            {
                _newRequestSleepInterval = value;
                _exitWaitInterval = _defaultExitWaitInterval + value;
            }
        }
        /// <summary>
        ///  退出前，等待时间(毫秒)，默认15s
        /// </summary>
        public int ExitWaitInterval
        {
            get => _exitWaitInterval;
            set => _exitWaitInterval = value;
        }
        /// <summary>
        ///  爬虫运行状态
        /// </summary>
        public SpiderStatus Status
        {
            get => _spiderStatus;
            private set
            {
                _spiderStatus = value;
                this.OnStatusChanged?.Invoke(this, _spiderStatus);
            }
        }

        /// <summary>
        ///  Identity
        /// </summary>
        public string Identity { get; set; }

        /// <summary>
        ///  线程数
        /// </summary>
        public int ThreadNumber
        {
            get => _threadNumber;
            set
            {
                if (_spiderStatus == SpiderStatus.Init)
                {
                    _threadNumber = value;
                }
                else
                {
                    throw new SpiderException("当前状态不允许设置");
                }
            }
        }

        public ILogger Logger { get; private set; }

        public IList<IPipeline> Pipelines { get; } = new List<IPipeline>();

        public IList<IPageProcessor> PageProcessors { get; } = new List<IPageProcessor>();

        public IScheduler Scheduler { get => _scheduler; private set => _scheduler = value; }

        public IDownloader Downloader { get => _downloader; private set => _downloader = value; }


        public IHtmlElementSelectorFactory SelectorFactory { get; private set; }


        /// <summary>
        ///  数据结果缓冲数量大小
        /// </summary>
        public int ResultItemsCacheCount
        {
            get => _resultItemsCacheCount;
            set
            {
                _resultItemsCacheCount = value;
            }
        }

        /// <summary>
        ///  监控程序
        /// </summary>
        public IMonitor Monitor { get; private set; }

        /// <summary>
        ///  队列监控程序
        /// </summary>
        public ISchedulerMonitor SchedulerMonitor { get; private set; }



        public Func<Request, Response> OnHandleRequestDownload { get; set; }

        #region event

        /// <summary>
        ///  当关闭的时候
        /// </summary>
        public event Action<Spider> OnClosed;
        /// <summary>
        ///  当状态发生改变的时候
        /// </summary>
        public event Action<Spider, SpiderStatus> OnStatusChanged;
        /// <summary>
        ///  当启动的时候
        /// </summary>
        public event Action<Spider> OnStarted;


        public event Action<Spider, Type> OnError;
        public event Action<Spider, Type> OnSuccess;

        /// <summary>
        ///  发送监控状态数据
        /// </summary>
        public event Action<Spider, MonitorData> OnMonitorReport;


        #endregion

        #region ctor

        /// <summary>
        /// ctor
        /// </summary>
        public Spider() : this(null, null, null)
        {
        }

        /// <summary>
        ///  爬虫
        /// </summary> 
        public Spider(string identity, IEnumerable<IPageProcessor> pageProcessors, IEnumerable<IPipeline> pipelines)
        {
            this.Identity = identity ?? Guid.NewGuid().ToString();

            if (pageProcessors != null)
                foreach (var item in pageProcessors)
                {
                    this.PageProcessors.Add(item);
                }


            if (pipelines != null)
                foreach (var item in pipelines)
                {
                    this.Pipelines.Add(item);
                }


            Init();
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///  添加请求
        /// </summary> 
        public Spider AddRequest(Request request)
        {
            if (Scheduler == null)
                _initRequest.Add(request);
            else
                this.Scheduler.Push(request);

            return this;
        }

        /// <summary>
        ///  添加请求
        /// </summary> 
        public Spider AddRequest(string url, string referer = null, Dictionary<string, string> exts = null)
        {
            var request = new Request(new Uri(url)) { Referer = referer };

            return AddRequest(request);
        }

        /// <summary>
        ///  添加请求
        /// </summary> 
        public Spider AddRequest(Uri uri, string referer = null, Dictionary<string, string> exts = null)
        {
            var request = new Request(uri) { Referer = referer };

            if (Scheduler == null)
                _initRequest.Add(request);
            else
                this.Scheduler.Push(request);

            return this;
        }

        /// <summary>
        ///  添加数据处理管道
        /// </summary> 
        public Spider AddPipeline(IPipeline pipeline)
        {
            pipeline.Logger = _loggerFactory.CreateLogger(pipeline.GetType().Name);

            this.Pipelines.Add(pipeline);
            return this;
        }

        /// <summary>
        ///  添加页面处理程序
        /// </summary> 
        public Spider AddPageProcessor(IPageProcessor pageProcessor)
        {
            pageProcessor.Logger = _loggerFactory.CreateLogger(pageProcessor.GetType().Name);

            this.PageProcessors.Add(pageProcessor);
            return this;
        }

        /// <summary>
        ///  设置新的日志工厂
        /// </summary> 
        public Spider SetLogFactory(ILoggerFactory loggerFactory)
        {
            this._loggerFactory = loggerFactory;
            return this;
        }

        /// <summary>
        ///  添加 日志 管道
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public Spider AddLogProvider(ILoggerProvider provider)
        {
            _loggerFactory.AddProvider(provider);

            return this;
        }

        /// <summary>
        ///  设置队列
        /// </summary> 
        public Spider SetScheduler(IScheduler scheduler, ISchedulerMonitor schedulerMonitor)
        {
            SetScheduler(scheduler);

            if (schedulerMonitor != null)
            {
                this.SchedulerMonitor = schedulerMonitor;
            }

            return this;
        }

        /// <summary>
        ///  设置队列
        /// </summary> 
        public Spider SetScheduler(IScheduler scheduler)
        {
            this.Scheduler = scheduler;

            if (scheduler is ISchedulerMonitor monitor)
            {
                this.SchedulerMonitor = monitor;
            }

            return this;
        }

        /// <summary>
        ///  设置下载器
        /// </summary> 
        public Spider SetDownloader(IDownloader downloader)
        {
            this.Downloader = downloader;
            return this;
        }

        /// <summary>
        ///  设置下载器
        /// </summary> 
        public Spider SetDownloader(IDownloader downloader, IDownloaderProxy downloaderProxy)
        {
            this.Downloader = downloader;

            if (downloaderProxy != null)
                _downloaderProxy = downloaderProxy;

            return this;
        }

        /// <summary>
        ///  设置下载器代理
        /// </summary> 
        public Spider SetDownloaderProxy(IDownloaderProxy downloaderProxy)
        {
            _downloaderProxy = downloaderProxy;

            return this;
        }

        /// <summary>
        ///  设置下载器代理
        /// </summary> 
        public Spider SetDownloaderProxy(WebProxy proxy)
        {
            _downloaderProxy = new DownloaderProxy(proxy);

            return this;
        }

        /// <summary>
        ///  设置下载前置程序
        /// </summary> 
        public Spider AddDownloadBeforeHandle(IDownloadBeforeHandle handle)
        {
            this._downloadBeforeHandles.Add(handle);

            return this;
        }

        /// <summary>
        ///  设置下载后置程序
        /// </summary> 
        public Spider AddDownloadAfterHandle(IDownloadAfterHandle handle)
        {
            this._downloadAfterHandles.Add(handle);

            return this;
        }

        /// <summary>
        ///  设置 监控程序
        /// </summary> 
        public Spider SetMonitor(IMonitor monitor)
        {
            this.Monitor = monitor;

            return this;
        }

        /// <summary>
        ///  使用HttpClient下载器
        /// </summary> 
        public Spider UseHttpClientDownloader(bool value = true)
        {
            this._useHttpClientDownloader = value;

            return this;
        }

        /// <summary>
        ///  设置最大线程数
        /// </summary> 
        public Spider SetMaxThreadNumber(int value)
        {
            this.ThreadNumber = value;
            return this;
        }





        /// <summary>
        ///  启动爬虫，但不占用当前进程
        /// </summary>
        public virtual void Start()
        {
            if (Status == SpiderStatus.Init || Status == SpiderStatus.Exited)
            {
                Task.Factory.StartNew(Run);
            }
            else
            {
                this.Logger.Warn("当前已启动，无需再次启动");
            }
        }

        /// <summary>
        ///  在当前进程启动爬虫
        /// </summary>
        public virtual void Run()
        {
            if (Status != SpiderStatus.Init && Status != SpiderStatus.Exited)
            {
                this.Logger.Warn("当前已启动，无需再次启动");
                return;
            }

            RunBefore();

            this.Logger.Info("已启动爬虫");

            this.OnStarted?.Invoke(this);

            Status = SpiderStatus.Running;

            // 上报状态
            ReportMonitorStatus();

            RunCore();

            // 清空缓存结果
            FlushResultItemsCache();

            // 上报状态
            ReportMonitorStatus();


            if (Status == SpiderStatus.Finished)
            {
                Status = SpiderStatus.Exited;

                this.Closed();
            }
        }

        /// <summary>
        ///  继续
        /// </summary>
        public virtual void Contiune()
        {
            if (Status != SpiderStatus.Paused)
            {
                Logger.Warn("当前状态无法继续。");
            }
            else
            {
                Status = SpiderStatus.Running;
                Logger.Info("继续运行。");
            }
        }

        /// <summary>
        ///  暂停
        /// </summary>
        public virtual void Pause()
        {
            if (Status != SpiderStatus.Running)
            {
                Logger.Warn("当前状态无法暂停。");
            }
            else
            {
                Status = SpiderStatus.Paused;
                Logger.Info("已暂停。");
            }
        }

        /// <summary>
        ///  停止
        /// </summary>
        public virtual void Stop()
        {
            if (this.Status == SpiderStatus.Running || this.Status == SpiderStatus.Paused)
            {
                this.Status = SpiderStatus.Finished;
                Logger.Info("正在停止...");
            }
            else
            {
                Logger.Info("已退出。");
            }
        }


        #endregion



        private void WaitNewRequest(ref int count)
        {
            Thread.Sleep(10);
            ++count;
        }

        /// <summary>
        ///  初始化
        /// </summary>
        private void Init()
        {
            if (_init) return;
            _init = true;

            InitDefaults();

            WelcomeInfo();

            Logger.Info("正在初始化...");
        }

        /// <summary>
        ///  初始化默认值
        /// </summary>
        private void InitDefaults()
        {
            if (this.Monitor == null)
            {
                this.Monitor = new LogMonitor()
                {
                    Logger = _loggerFactory.CreateLogger(typeof(LogMonitor).Name)
                };
            }

            if (this.Downloader == null)
            {
                this.Downloader = new DefaultHttpClientDownloader()
                {
                    Logger = _loggerFactory.CreateLogger(typeof(DefaultHttpClientDownloader).Name)
                };
            }
        }


        /// <summary>
        ///  运行前
        /// </summary>
        private void RunBefore()
        {
            InitLog();

            if (this.Scheduler == null)
            {
                var q = new SampleQueueScheduler() { Logger = _loggerFactory.CreateLogger(typeof(SampleQueueScheduler).Name) };
                this.Scheduler = q;
                this.SchedulerMonitor = q;
            }
            else
            {
                this.Scheduler.Logger = _loggerFactory.CreateLogger(this.Scheduler.GetType().Name);
            }

            if (_initRequest.Count > 0)
            {
                foreach (var request in _initRequest)
                {
                    this.Scheduler.Push(request);
                }
            }

            if (this.Downloader == null && _useHttpClientDownloader)
                this.Downloader = new DefaultHttpClientDownloader()
                {
                    Logger = _loggerFactory.CreateLogger(typeof(DefaultHttpClientDownloader).Name)
                };

            if (_downloadBeforeHandles.Count > 0)
            {
                foreach (var handle in _downloadBeforeHandles)
                {
                    handle.Logger = _loggerFactory.CreateLogger(handle.GetType().Name);
                    this.Downloader.AddDownloadBeforeHandle(handle);
                }
            }

            if (_downloadAfterHandles.Count > 0)
            {
                foreach (var handle in _downloadAfterHandles)
                {
                    handle.Logger = _loggerFactory.CreateLogger(handle.GetType().Name);
                    this.Downloader.AddDownloadAfterHandle(handle);
                }
            }

            if (this.Pipelines.Count == 0)
                this.Pipelines.Add(new ConsolePipeline()
                {
                    Logger = _loggerFactory.CreateLogger(typeof(ConsolePipeline).Name)
                });

            InitHtmlQuery();

            _monitorThread = new Thread(ReportMonitorStatusProcess)
            {
                IsBackground = true,
                Name = "Spider Status Report Thread"
            };
            _monitorThread.Start();

            OnRunBefore();
        }


        protected virtual void OnRunBefore()
        { }

        private void InitLog()
        {
            this.Logger = _loggerFactory.CreateLogger(typeof(Spider).Name);

            foreach (var pipeline in Pipelines)
            {
                pipeline.Logger = _loggerFactory.CreateLogger(pipeline.GetType().Name);
            }
            foreach (var processor in PageProcessors)
            {
                processor.Logger = _loggerFactory.CreateLogger(processor.GetType().Name);
            }
        }

        #region HtmlQuery

        private void InitHtmlQuery()
        {
            // TODO: 更优雅的方法 ???  

            if (this.SelectorFactory != null)
                return;

            TryLoadHtmlAgilityPackHtmlSelectorFactory();

            if (this.SelectorFactory == null)
                TryLoadAngleSharpHtmlSelectorFactory();
        }

        private void TryLoadHtmlAgilityPackHtmlSelectorFactory()
        {
            try
            {
                var ass = Assembly.Load("DotnetSpiderLite.HtmlAgilityPack");
                var type = ass.GetType("DotnetSpiderLite.HtmlAgilityPack.HtmlElementSelectorFactory");

                var typeObj = Activator.CreateInstance(type);

                this.SelectorFactory = typeObj as IHtmlElementSelectorFactory;

                this.Logger.Trace("Load type 'HtmlAgilityPack.HtmlElementSelectorFactory' ");
            }
            catch (Exception)
            {
                this.Logger.Trace("Can't load type 'HtmlAgilityPack.HtmlElementSelectorFactory' ");
            }
        }

        private void TryLoadAngleSharpHtmlSelectorFactory()
        {
            try
            {
                var ass = Assembly.Load("DotnetSpiderLite.AngleSharp");
                var type = ass.GetType("DotnetSpiderLite.AngleSharps.HtmlElementSelectorFactory");

                var typeObj = Activator.CreateInstance(type);

                this.SelectorFactory = typeObj as IHtmlElementSelectorFactory;

                this.Logger.Trace("Load type 'AngleSharp.HtmlElementSelectorFactory' ");
            }
            catch (Exception)
            {
                this.Logger.Trace("Can't load type 'AngleSharp.HtmlElementSelectorFactory' ");
            }
        }

        #endregion


        private void RunCore()
        {
            while (Status == SpiderStatus.Running || Status == SpiderStatus.Paused)
            {
                if (Status == SpiderStatus.Paused)
                {
                    Thread.Sleep(10);
                    continue;
                };

                Parallel.For(0,
                    _threadNumber,
                    new ParallelOptions() { MaxDegreeOfParallelism = _threadNumber },
                    (_) =>
                    {
                        int waitCount = 1;

                        while (Status == SpiderStatus.Running)
                        {
                            // 取出 
                            var request = Scheduler.Pull();

                            if (request == null)
                            {
                                // 等待超时，退出
                                if (waitCount > _waitCountLimit)
                                {
                                    Status = SpiderStatus.Finished;
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

                                    Thread.Sleep(NewRequestSleepInterval);
                                }
                                catch
                                {
                                }

                            }
                        }

                    });

            }

        }

        protected virtual bool OnHandleRequesting(Request request, IDownloader downloader)
        {
            return true;
        }

        private async Task HandleRequestAsync(Request request, IDownloader downloader)
        {
            if (!OnHandleRequesting(request, downloader))
            {
                return;
            }

            // 下载页面 
            var response = await HandleDownloadAsync(request, downloader);

            if (response == null)
                return;

            var page = new Page(response);
            page.Extra.Marge(request.Extra);
            page.Extra["Downloader"] = downloader.GetType().FullName;
            page.Extra["Identity"] = this.Identity;

            if (SelectorFactory != null)
            {
                page.SetSelector(SelectorFactory.GetSelector(page.Content));
                page.Extra["Selector"] = page.Selector.GetType().FullName;
            }


            // 页面处理程序
            HandlePageProcessors(page);


            if (page.Skip)
                return;

            // 重试处理
            if (page.Retry)
            {
                if (page.MaxRetryTimes > 0 && request.GetRetryCount() > page.MaxRetryTimes)
                {
                    return;
                }

                request.IncrementRetryCount();

                // 添加到 队列 
                this.Scheduler.Push(request);
                return;
            }


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

        protected virtual void OnHandleDownloading(Request request, IDownloader downloader) { }


        private async Task<Response> HandleDownloadAsync(Request request, IDownloader downloader)
        {
            var uri = request.Uri;
            this.Logger.Trace($"下载器开始请求URL：{uri} ");

            try
            {
                var sw = Stopwatch.StartNew();

                if (OnHandleRequestDownload != null)
                {
                    return OnHandleRequestDownload.Invoke(request);
                }
                else
                {
                    WebProxy proxy = null;
                    if (_downloaderProxy != null)
                    {
                        proxy = _downloaderProxy.GetProxy();
                        if (proxy != null)
                        {
                            downloader.Proxy = proxy;

                            Logger.Trace($"使用代理 {proxy.Address}");
                        }
                    }

                    var response = await downloader.DownloadAsync(request);

                    if (_downloaderProxy != null && proxy != null)
                    {
                        _downloaderProxy.RelaseProxy(proxy, response);
                    }

                    sw.Stop();
                    CalculateDownloadSpeed(sw.ElapsedMilliseconds);

                    this.SchedulerMonitor?.IncreaseSuccessCount();
                    this.OnSuccess?.Invoke(this, typeof(IDownloader));

                    return response;
                }
            }
            catch (DownloaderException ex)
            {
                // TODO 

                this.Logger.Error(ex, $"下载失败。URL：{uri}");

                this.OnError?.Invoke(this, typeof(IDownloader));

                this.SchedulerMonitor?.IncreaseErrorCount();
            }
            catch (Exception ex)
            {
                this.Logger.Error(ex, $"下载失败。URL：{uri}");

                this.OnError?.Invoke(this, typeof(IDownloader));

                this.SchedulerMonitor?.IncreaseErrorCount();
            }

            return null;
        }


        protected virtual void OnHandlePageProcessorsing(Page page)
        {
        }

        private void HandlePageProcessors(Page page)
        {
            var sw = Stopwatch.StartNew();

            OnHandlePageProcessorsing(page);

            foreach (var processor in PageProcessors)
            {
                try
                {
                    processor.Process(page);

                    this.OnSuccess?.Invoke(this, processor.GetType());
                }
                catch (Exception ex)
                {
                    this.OnError?.Invoke(this, processor.GetType());

                    this.Logger.Error(ex, $"处理器'{processor.GetType().FullName}'执行失败。");
                }
            }

            CalculateProcessorSpeed(sw.ElapsedMilliseconds);
        }


        protected virtual void OnHandlePipelineing(IList<ResultItems> items)
        {
        }

        private void HandlePipelines(Page page)
        {
            var list = new List<ResultItems>();

            if (this._resultItemsCacheCount > 0)
            {
                lock (_lockObject)
                {
                    if (_cacheResultItems.Count < this._resultItemsCacheCount)
                    {
                        _cacheResultItems.Add(page.ResutItems);
                        return;
                    }
                    else
                    {
                        list.AddRange(_cacheResultItems);
                        _cacheResultItems.Clear();
                    }
                }
            }
            else
            {
                list.Add(page.ResutItems);
            }

            SendToPipelines(list);

        }

        private void FlushResultItemsCache()
        {
            if (_cacheResultItems.Count > 0)
            {
                SendToPipelines(_cacheResultItems);
            }
        }

        private void SendToPipelines(IList<ResultItems> list)
        {
            if (list == null || list.Count == 0)
                return;

            var sw = Stopwatch.StartNew();

            OnHandlePipelineing(list);

            foreach (var pipeline in Pipelines)
            {
                try
                {
                    pipeline.Process(list);

                    this.OnSuccess?.Invoke(this, pipeline.GetType());
                }
                catch (Exception ex)
                {
                    this.OnError?.Invoke(this, pipeline.GetType());

                    this.Logger.Error(ex, $"管道'{pipeline.GetType().FullName}'执行失败。");
                }

            }

            sw.Stop();
            CalculatePipelineSpeed(sw.ElapsedMilliseconds);

        }





        [MethodImpl(MethodImplOptions.Synchronized)]
        private void CalculateDownloadSpeed(long time)
        {
            _downloaderCounts++;
            _downloaderCostTimes += time;
            _downloadAvgSpeed = _downloaderCostTimes / _downloaderCounts;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void CalculateProcessorSpeed(long time)
        {
            _processorCounts++;
            _processorCostTimes += time;
            _processorAvgSpeed = _processorCostTimes / _processorCounts;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void CalculatePipelineSpeed(long time)
        {
            _pipelineCounts++;
            _pipelineCostTimes += time;
            _pipelineAvgSpeed = _pipelineCostTimes / _pipelineCounts;
        }


        private void ReportMonitorStatusProcess()
        {
            while (true)
            {
                if (Status == SpiderStatus.Running)
                {
                    // 上报状态
                    ReportMonitorStatus();
                }

                Thread.Sleep(TimeSpan.FromSeconds(2));
            }
        }

        /// <summary>
        ///  上报状态
        /// </summary>
        private void ReportMonitorStatus()
        {
            MonitorData data = new MonitorData()
            {
                Identity = Identity,
                NodeId = Identity,
                Status = Status.ToString(),
                Thread = _threadNumber,

                AvgDownloadSpeed = _downloadAvgSpeed,
                AvgPipelineSpeed = _pipelineAvgSpeed,
                AvgProcessorSpeed = _processorAvgSpeed,
            };

            if (SchedulerMonitor != null)
            {
                data.Error = SchedulerMonitor.ErrorRequestsCount;
                data.Left = SchedulerMonitor.LeftRequestsCount;
                data.Success = SchedulerMonitor.SuccessRequestsCount;
                data.Total = SchedulerMonitor.TotalRequestsCount;
            }

            if (this.Monitor != null)
            {
                try
                {
                    this.Monitor?.Report(data);
                }
                catch (Exception ex)
                {
                    this.OnError?.Invoke(this, Monitor.GetType());

                    this.Logger.Error(ex, "发送当前状态数据失败");
                }
            }

            if (this.OnMonitorReport != null)
            {
                try
                {
                    OnMonitorReport?.Invoke(this, data);
                }
                catch (Exception ex)
                {
                    this.OnError?.Invoke(this, OnMonitorReport.GetType());

                    this.Logger.Error(ex, "发送当前状态数据失败");
                }
            }
        }




        private void Closed()
        {
            this.OnClosed?.Invoke(this);
            this.Logger.Info("正在停止...");

            SafeDestroy();

            this.Logger.Info("已停止");
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

#if !NETSTANDARD
            _monitorThread?.Abort();
#endif

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




        void WelcomeInfo()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("==================================================");
            sb.AppendLine("===                                            ===");
            sb.AppendLine("=== Welcome to Dotnet Spider Lite              ===");
            sb.AppendLine("=== Dotnet Spider Lite An open source crawler  ===");
            sb.AppendLine("===                                            ===");
            sb.AppendLine("==================================================");

            Console.WriteLine(sb.ToString());

            this.Logger.Info("正在启动...");
        }

    }
}
