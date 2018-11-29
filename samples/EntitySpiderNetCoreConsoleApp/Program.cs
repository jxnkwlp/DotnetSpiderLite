using DotnetSpiderLite.Entity;
using DotnetSpiderLite.Entity.Attributes;
using DotnetSpiderLite.Html;
using System;

namespace EntitySpiderNetCoreConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var spider = EntitySpider.Create<CnblogsArticleModel>("https://www.cnblogs.com/");
            // spider.AddEntity<CnblogsListModel>();

            spider.Run();


            Console.ReadKey();
        }

        [HelperUrl("https://www.cnblogs.com/")]
        [TargetUrl("https://www.cnblogs.com/*")]
        public class CnblogsArticleModel : IEntity
        {
            [Selector(".postTitle a", HtmlSelectorPathType.Css)]
            public string Title { get; set; }

            [Selector(".postTitle a", HtmlSelectorPathType.Css, FromAttribute = "href")]
            public string Link { get; set; }


            [Selector("#cnblogs_post_body", HtmlSelectorPathType.Css, SelectorMatch = SelectorMatch.InnerHtml)]
            public string Content { get; set; }

        }
    }
}
