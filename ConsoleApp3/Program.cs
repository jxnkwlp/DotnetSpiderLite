using DotnetSpiderLite;
using DotnetSpiderLite.Helper;
using DotnetSpiderLite.Html;
using DotnetSpiderLite.PageProcessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //Spider spider = Spider.Create("https://www.cnblogs.com/");
            //spider.UseNLog();
            //spider.AddPageProcessors(new CNBlogProcessor());
            //spider.AddPipelines(new FilePipeline());
            //spider.AddPipelines(new JsonFilePipeline()); 
            //spider.ThreadNumber = 10;

            Spider spider = Spider.Create("https://weixin.sogou.com/");
            //spider.UseNLog();

            spider.AddPageProcessors(new Processor2());


            spider.AddRequest("https://weixin.sogou.com/weixin?type=2&ie=utf8&query=马云");



            spider.Run();
            //spider.Start();

            spider.OnStatusChanged += Spider_OnStatusChanged;

            // Console.WriteLine("end main ");
        }

        private static void Spider_OnStatusChanged(Spider arg1, SpiderStatus arg2)
        {
            Console.WriteLine("status changed : " + arg2.ToString());
        }
    }


    public class Processor2 : BasePageProcessor
    {
        public override void HandlePage(Page page)
        {
            if (page.Response.ResponseUri.ToString() == "https://weixin.sogou.com/")
            {
                Console.WriteLine("页面已回到主页面：" + page.Response.ResponseUri);
                return;
            }

            if (page.Response.ResponseUri != page.Response.Request.Uri)
            {
                Console.WriteLine("页面已重定向：" + page.Response.ResponseUri);
                return;
            }

            if (page.Response.ResponseUri.Query != null && !page.Response.ResponseUri.Query.Contains("tsn=1"))
            {
                // type=2&ie=utf8&query=马云
                // type=2&ie=utf8&query=马云&tsn=1&ft=&et=&interation=&wxid=&usip=

                var url = page.Response.ResponseUri.ToString() + "&tsn=1&ft=&et=&interation=&wxid=&usip=";

                page.AddTargetRequest(url, page.Response.ResponseUri.ToString());

                return;
            }


            // 页码
            page.Selector.SelectorAll("#pagebar_container a", HtmlSelectorPathType.Css);


            var list = page.Selector.SelectorAll(".news-box .news-list li", HtmlSelectorPathType.Css);
            foreach (var item in list)
            {
                var titleEle = item.Selector(".txt-box h3 a", HtmlSelectorPathType.Css);

                Console.WriteLine("title:" + titleEle?.InnerText + " href:" + titleEle?.Attributes["href"]);


            }


        }
    }


    //public class CNBlogProcessor : BasePageProcessor
    //{
    //    public override void HandlePage(Page page)
    //    {
    //        var listEle = page.Selector.SelectorAll(".post_item", HtmlSelectorPathType.Css);

    //        // 列表页面 
    //        if (listEle != null && listEle.Count > 0)
    //        {
    //            foreach (var item in listEle)
    //            {
    //                var title = item.Selector(".titlelnk", HtmlSelectorPathType.Css);
    //                var href = title.Attributes["href"];

    //                Console.WriteLine($"列表:{title.InnerHtml} {href}");

    //                page.AddTargetRequest(href);
    //            }
    //        }
    //        else
    //        {
    //            Console.WriteLine(page.Response.Request.Uri);

    //            var title = page.Selector.Selector(".postTitle a", HtmlSelectorPathType.Css);
    //            var body = page.Selector.Selector("#cnblogs_post_body", HtmlSelectorPathType.Css);

    //            page.AddResultItem("内容title", title?.InnerText?.Trim());
    //            //page.AddResultItem("content", body?.InnerHtml?.Trim()); 
    //        }
    //    }

    //}
}
