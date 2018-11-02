using DotnetSpiderLite.Abstractions;
using DotnetSpiderLite.Abstractions.PageProcessor;
using DotnetSpiderLite.Core;
using System;
using System.Threading.Tasks;

using DotnetSpiderLite.HtmlAgilityPack;


namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Spider spider = new Spider();

            spider.AddRequest("https://github.com/jxnkwlp");

            spider.AddPageProcessors(new GithubProfileProcessor());

            spider.Run();

        }

        public class GithubProfileProcessor : BasePageProcessor
        {
            public override Task Handle(Page page)
            {
                page.AddResultItem("author", page.SelectSingle("//div[@class='p-nickname vcard-username d-block']"));
                page.AddResultItem("name", page.SelectSingle("//span[@class='p-name vcard-fullname d-block"));

                return Task.CompletedTask;
            }
        }
    }
}
