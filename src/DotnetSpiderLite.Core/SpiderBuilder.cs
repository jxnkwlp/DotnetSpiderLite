using DotnetSpiderLite.PageProcessor;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite
{
    /// <summary>
    ///  Spider 构建
    /// </summary>
    public class SpiderBuilder
    {
        private Spider _spider;

        /// <summary>
        ///  初始化一个<see cref="SpiderBuilder"/> 的新实列
        /// </summary> 
        public static SpiderBuilder CreateBuilder()
        {
            return new SpiderBuilder();
        }

        /// <summary>
        ///  添加一个初始请求
        /// </summary> 
        public SpiderBuilder AddRequest(Request request)
        {
            _spider.AddRequest(request);
            return this;
        }

        /// <summary>
        ///  添加一个初始请求
        /// </summary> 
        public SpiderBuilder AddRequest(string url, string referer = null)
        {
            _spider.AddRequest(url, referer);
            return this;
        }

        /// <summary>
        ///  添加一个或多个页面处理器
        /// </summary> 
        public SpiderBuilder AddPageProcessors(params IPageProcessor[] processors)
        {
            foreach (var item in processors)
            {
                _spider.AddPageProcessor(item);
            }

            return this;
        }

        /// <summary>
        ///  添加一个页面处理器
        /// </summary> 
        public SpiderBuilder AddPageProcessor(IPageProcessor processor)
        {
            _spider.AddPageProcessor(processor);

            return this;
        }

        /// <summary>
        ///  返回当前<see cref="Spider"/> 的实列
        /// </summary>
        /// <returns></returns>
        public Spider Buid()
        {
            return _spider;
        }

        /// <summary>
        ///  ctor
        /// </summary>
        protected SpiderBuilder()
        {
            _spider = new Spider();
        }

    }
}
