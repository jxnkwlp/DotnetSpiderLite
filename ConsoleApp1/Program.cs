using DotnetSpiderLite;
using DotnetSpiderLite.Abstractions;
using DotnetSpiderLite.Abstractions.PageProcessor;
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

            spider.Run();

        }

        public class GithubProfileProcessor : BasePageProcessor
        {
            public override Task HandlePage(Page page)
            {
                var httpStatusCode = page.Response.StatusCode;

                //page.AddResultItem("author", page.SelectSingle("//div[@class='p-nickname vcard-username']"));
                //page.AddResultItem("name", page.SelectSingle("//span[@class='p-name vcard-fullname']"));

                page.AddResultItem("author", page.SelectSingleByCss(".p-nickname.vcard-username"));
                page.AddResultItem("name", page.SelectSingleByCss(".p-name.vcard-fullname"));


                return Task.CompletedTask;
            }
        }


        public class CNBlogProcessor : BasePageProcessor
        {
            public override Task HandlePage(Page page)
            {
                if (page.SelectSingleByCss("#post_list") != null)
                {
                    // 列表页面 
                    var list = page.SelectAllByCss("#post_list .post_item");

                    for (int i = 0; i < list.Count; i++)
                    {
                        var title = page.SelectSingleByCss("#post_list .post_item:eq(" + i + ") .titlelnk");

                        var href = title.Attributes["href"];

                        Console.WriteLine(title.InnerHtml + " " + href);

                        page.AddTargetRequest(href);
                    }
                }
                else
                {
                    Console.WriteLine(page.Response.Request.Uri);

                }

                return Task.CompletedTask;
            }

        }

    }
}
