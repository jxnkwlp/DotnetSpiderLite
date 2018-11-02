using DotnetSpider.Core;
using DotnetSpider.Core.Processor;
using System;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //var site = new Site { CycleRetryTimes = 3, SleepTime = 300 };
            var spider = Spider.Create(new GithubProfileProcessor()).AddRequest("https://github.com/zlzforever");
            spider.ThreadNum = 5;
            spider.Run();

        }

        private class GithubProfileProcessor : BasePageProcessor
        {
            protected override void Handle(Page page)
            {
                page.AddResultItem("author", page.Selectable().XPath("//div[@class='p-nickname vcard-username d-block']").GetValue());
                var name = page.Selectable().XPath("//span[@class='p-name vcard-fullname d-block']").GetValue();
                page.SkipTargetRequests = string.IsNullOrWhiteSpace(name);
                page.AddResultItem("name", name);
                page.AddResultItem("bio", page.Selectable().XPath("//div[@class='p-note user-profile-bio']/div").GetValue());
            }
        }

    }

    
}
