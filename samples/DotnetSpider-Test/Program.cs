using DotnetSpider.Core;
using DotnetSpider.Core.Processor;
using System;

namespace DotnetSpider_Test
{
    class Program
    {
        static void Main(string[] args)
        {

            var spider = Spider.Create(new GithubProfileProcessor()).AddRequest("https://github.com/zlzforever", null);
            spider.ThreadNum = 5;
            spider.Run();

            Console.Read();
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
