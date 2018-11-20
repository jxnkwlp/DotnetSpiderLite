using DotnetSpiderLite;
using DotnetSpiderLite.Html;
using DotnetSpiderLite.PageProcessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class WeixinSougouTest
    {
        public void Run()
        {
            Spider spider = Spider.Create("https://weixin.sogou.com/");
            //spider.UseNLog();

            spider.AddPageProcessor(new Processor2());

            // spider.AddRequest($"https://weixin.sogou.com/weixin?type=2&ie=utf8&query=马云");

            for (int i = 1; i <= 3; i++)
            {
                spider.AddRequest($"https://weixin.sogou.com/weixin?type=2&ie=utf8&page={i}&query=马云");
            }



            spider.NewRequestSleepInterval = 2000; // 2s
            // spider.EmptySleepTime = 60; // 60s

            spider.Run();
            //spider.Start();

            spider.OnStatusChanged += Spider_OnStatusChanged;

            // Console.WriteLine("end main ");
        }

        private static void Spider_OnStatusChanged(Spider arg1, SpiderStatus arg2)
        {
            Console.WriteLine("status changed : " + arg2.ToString());
        }


        public class Processor2 : BasePageProcessor
        {
            public override bool CanProcess(Page page)
            {
                return base.CanProcess(page);
            }

            public override void HandlePage(Page page)
            {
                if (page.Response.ResponseUri.AbsolutePath == "/antispider/")
                {

                    // 验证码识别

                    Console.WriteLine("验证码识别...");

                    page.Retry = true;
                    return;
                }

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


                //// 页码
                //var pageLinks = page.Selector.SelectorAll("#pagebar_container a", HtmlSelectorPathType.Css);

                //foreach (var item in pageLinks)
                //{
                //    string pageUrl = "https://weixin.sogou.com/weixin" + item.Attributes["href"];
                //    page.AddTargetRequest(pageUrl, page.Response.ResponseUri.ToString());
                //}


                var list = page.Selector.SelectorAll(".news-box .news-list li", HtmlSelectorPathType.Css);
                foreach (var item in list)
                {
                    var titleEle = item.Selector(".txt-box h3 a", HtmlSelectorPathType.Css);

                    Console.WriteLine("title:" + titleEle?.InnerText + " href:" + titleEle?.Attributes["href"]);

                }


            }
        }



    }
}
