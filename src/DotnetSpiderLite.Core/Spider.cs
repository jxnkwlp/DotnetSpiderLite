using DotnetSpiderLite.Abstractions.Downloader;
using DotnetSpiderLite.Abstractions.PageProcessor;
using DotnetSpiderLite.Abstractions.Pipeline;
using DotnetSpiderLite.Abstractions.Scheduler;
using DotnetSpiderLite.Core.Downloader;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;

namespace DotnetSpiderLite.Core
{
    /// <summary>
    ///  main
    /// </summary>
    public class Spider
    {
        private IScheduler _scheduler;
        private IDownloader _downloader = new DefaultHttpClientDownloader();



        public string Identity { get; set; }


        public IList<IPipeline> Pipelines { get; private set; } = new List<IPipeline>();

        public IList<IPageProcessor> PageProcessors { get; private set; } = new List<IPageProcessor>();


        public IScheduler Scheduler { get => _scheduler; set => _scheduler = value; }
        public IDownloader Downloader { get => _downloader; set => _downloader = value; }




        #region ctor

        public Spider() : this(null, null, null)
        {
             
        }

        public Spider(string identity, IEnumerable<IPageProcessor> pageProcessors, IEnumerable<IPipeline> pipelines)
        {
            this.Identity = identity ?? Guid.NewGuid().ToString();

            if (pageProcessors != null)
                this.PageProcessors = new List<IPageProcessor>(pageProcessors);

            if (pipelines != null)
                this.Pipelines = new List<IPipeline>(pipelines);


        }

        #endregion


        #region Public Methods


        public void AddRequest(string url, Dictionary<string, string> exts = null)
        {

        }

        public void AddPipelines(IPipeline pipeline)
        {
            this.Pipelines.Add(pipeline);
        }

        public void AddPageProcessors(IPageProcessor pageProcessor)
        {
            this.PageProcessors.Add(pageProcessor);
        }



        public void Run()
        {

        }

        #endregion

    }
}
