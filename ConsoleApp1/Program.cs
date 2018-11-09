using DotnetSpiderLite;
using DotnetSpiderLite;
using DotnetSpiderLite.Html;
using DotnetSpiderLite.PageProcessor;
using DotnetSpiderLite.Pipeline;
using System;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //Spider spider = Spider.Create("https://github.com/jxnkwlp"); 
            //spider.AddPageProcessors(new GithubProfileProcessor());  


            Spider spider = Spider.Create("https://www.cnblogs.com/");
            spider.AddPageProcessors(new CNBlogProcessor());
            //spider.AddPipelines(new FilePipeline());
            //spider.AddPipelines(new JsonFilePipeline()); 
            //spider.ThreadNumber = 10;
            spider.Run();

        }

        //public class GithubProfileProcessor : BasePageProcessor
        //{
        //    public override Task HandlePage(Page page)
        //    {
        //        var httpStatusCode = page.Response.StatusCode;

        //        //page.AddResultItem("author", page.SelectSingle("//div[@class='p-nickname vcard-username']"));
        //        //page.AddResultItem("name", page.SelectSingle("//span[@class='p-name vcard-fullname']"));

        //        page.AddResultItem("author", page.SelectSingleByCss(".p-nickname.vcard-username"));
        //        page.AddResultItem("name", page.SelectSingleByCss(".p-name.vcard-fullname"));


        //        return Task.CompletedTask;
        //    }
        //}


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

                        Console.WriteLine($"列表:{title.InnerHtml} {href}");

                        page.AddTargetRequest(href);
                    }
                }
                else
                {
                    Console.WriteLine(page.Response.Request.Uri);

                    var title = page.Selector.Selector(".postTitle a", HtmlSelectorPathType.Css);
                    var body = page.Selector.Selector("#cnblogs_post_body", HtmlSelectorPathType.Css);

                    page.AddResultItem("title", title?.InnerText?.Trim());
                    //page.AddResultItem("content", body?.InnerHtml?.Trim()); 
                }
            }

        }

    }
}
