using DotnetSpider.Core;
using DotnetSpider.Core.Processor;
using DotnetSpider.Extension;
using DotnetSpider.Extension.Model;
using DotnetSpider.Extension.Pipeline;
using DotnetSpider.Extraction;
using DotnetSpider.Extraction.Model;
using DotnetSpider.Extraction.Model.Attribute;
using DotnetSpider.Extraction.Model.Formatter;
using System;
using System.Collections.Generic;

namespace DotnetSpider_Test
{
    class Program
    {
        static void Main(string[] args)
        {

            //var spider = Spider.Create(new GithubProfileProcessor()).AddRequest("https://github.com/zlzforever", null);
            //spider.ThreadNum = 5;
            //spider.Run();

            Spider spider = new Spider();
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

        private class Spider : EntitySpider
        {
            protected override void OnInit(params string[] arguments)
            {
                var word = "可乐|雪碧";

                AddRequest(string.Format("http://news.baidu.com/ns?word={0}&tn=news&from=news&cl=2&pn=0&rn=20&ct=1", word), new Dictionary<string, dynamic> { { "Keyword", word } });

                AddEntityType<BaiduSearchEntry>();
                AddPipeline(new ConsoleEntityPipeline());
                 

            }

            [Schema("baidu", "baidu_search_entity_model")]
            [Entity(Expression = ".//div[@class='result']", Type = SelectorType.XPath)]
            class BaiduSearchEntry : IBaseEntity
            {
                [Column]
                [Field(Expression = "Keyword", Type = SelectorType.Enviroment)]
                public string Keyword { get; set; }

                [Column]
                [Field(Expression = ".//h3[@class='c-title']/a")]
                [ReplaceFormatter(NewValue = "", OldValue = "<em>")]
                [ReplaceFormatter(NewValue = "", OldValue = "</em>")]
                public string Title { get; set; }

                [Column]
                [Field(Expression = ".//h3[@class='c-title']/a/@href")]
                public string Url { get; set; }

                [Column]
                [Field(Expression = ".//div/p[@class='c-author']/text()")]
                [ReplaceFormatter(NewValue = "-", OldValue = "&nbsp;")]
                public string Website { get; set; }

                [Column]
                [Field(Expression = ".//div/span/a[@class='c-cache']/@href")]
                public string Snapshot { get; set; }

                [Column]
                [Field(Expression = ".//div[@class='c-summary c-row ']", Option = FieldOptions.InnerText)]
                [ReplaceFormatter(NewValue = "", OldValue = "<em>")]
                [ReplaceFormatter(NewValue = "", OldValue = "</em>")]
                [ReplaceFormatter(NewValue = " ", OldValue = "&nbsp;")]
                public string Details { get; set; }

                [Column]
                [Field(Expression = ".", Option = FieldOptions.InnerText)]
                [ReplaceFormatter(NewValue = "", OldValue = "<em>")]
                [ReplaceFormatter(NewValue = "", OldValue = "</em>")]
                [ReplaceFormatter(NewValue = " ", OldValue = "&nbsp;")]
                public string PlainText { get; set; }
            }
        }
    }
}
