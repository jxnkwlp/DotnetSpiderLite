using DotnetSpiderLite.Abstractions;
using DotnetSpiderLite.Abstractions.PageProcessor;
using DotnetSpiderLite.Core;
using System;
using System.Threading.Tasks;

using DotnetSpiderLite.HtmlAgilityPack;
using System.Collections.Generic;
using DotnetSpiderLite.AngleSharps;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Spider spider = Spider.Create("https://github.com/jxnkwlp");

            spider.PageExtracter = new AngleSharpHtmlExtracter();

            spider.AddPageProcessors(new GithubProfileProcessor());

            spider.Run();

        }

        public class GithubProfileProcessor : BasePageProcessor
        {
            public override Task Handle(Page page)
            {
                try
                {
                    page.AddResultItem("author", page.SelectSingle("//div[@class='p-nickname vcard-username d-block']"));
                    page.AddResultItem("name", page.SelectSingle("//span[@class='p-name vcard-fullname d-block']"));

                }
                catch (Exception)
                {
                }

                return Task.CompletedTask;
            }
        }


        public class CNBlogProcessor : BasePageProcessor
        {
            public override IEnumerable<Request> ExtractRequest(Page page)
            {


                return base.ExtractRequest(page);
            }

            public override Task Handle(Page page)
            {
                return Task.CompletedTask;
            }

        }

    }
}
