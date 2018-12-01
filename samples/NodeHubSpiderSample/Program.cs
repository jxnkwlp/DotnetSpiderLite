using DotnetSpiderLite;
using DotnetSpiderLite.Html;
using DotnetSpiderLite.PageProcessor;
using System;

namespace NodeHubSpiderSample
{
    class Program
    {

        static void Main(string[] args)
        {

            Spider spider = Spider.Create("https://www.cnblogs.com/", new CNBlogProcessor());

            // var hubSpider = spider.AsNodeHubSpider();


        }

        public class CNBlogProcessor : BasePageProcessor
        {
            public override void HandlePage(Page page)
            {
                var listEle = page.Selector.SelectorAll(".post_item", HtmlSelectorPathType.Css);

                // 列表页面 
                if (listEle != null && listEle.Count > 0)
                {
                    foreach (var item in listEle)
                    {
                        var title = item.Selector(".titlelnk", HtmlSelectorPathType.Css);
                        var href = title.Attributes["href"];

                        page.AddTargetRequest(href);
                    }
                }
                else
                {
                    // Console.WriteLine(page.Response.Request.Uri);

                    var title = page.Selector.Selector(".postTitle a", HtmlSelectorPathType.Css);
                    var body = page.Selector.Selector("#cnblogs_post_body", HtmlSelectorPathType.Css);

                    page.AddResultItem("标题", title?.InnerText?.Trim());
                    //page.AddResultItem("内容", body?.InnerHtml?.Trim());  
                    page.AddResultItem("链接", page.Response.ResponseUri);

                }
            }

        }
    }
}
