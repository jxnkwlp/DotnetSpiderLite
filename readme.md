# DotnetSpiderLite

轻量级 dotnet 爬虫。复刻自 DotnetSpider

> 造这个轮子的原因是， DotnetSpider各个之间依赖太强了，本来想简单的采集一些东西，安装后却来了一个全家桶... 我的期望是各个组件之间尽可能的解耦，但需要的时候就安装这个组件。

## 名词

 1. Downloader : 下载器
 2. PageProcessor : 页面下载器
 3. Pipeline ： 数据管道
 4. Scheduler ： 队列

## TODO

 1. [ ] 完整注释
 2. [ ] UI管理界面
 3. [ ] 支持多机/多节点部署和管理界面
 4. [x] [使用 Redis 作为队列](#使用redis作为队列程序)
 5. [ ] 数据库 支持
 6. [ ] 支持使用代理

## 安装

i. 安装包 [DotnetSpiderLite.Core](https://www.nuget.org/packages/DotnetSpiderLite.Core/)

~~~ c#
PM> install-package DotnetSpiderLite.Core
~~~

ii. 安装html解析扩展包（可选，如果需求解析html，则需要安装） 目前实现了 [HtmlAgilityPack](https://www.nuget.org/packages/DotnetSpiderLite.HtmlAgilityPack/) 和 [AngleSharp](https://www.nuget.org/packages/DotnetSpiderLite.AngleSharp/)

~~~ c#
PM> install-package DotnetSpiderLite.HtmlAgilityPack
~~~

## 使用

~~~ c#
Spider spider = Spider.Create("https://www.cnblogs.com/");
spider.AddPageProcessor(new CNBlogProcessor());
//spider.AddPipeline(new FilePipeline());
//spider.AddPipeline(new JsonFilePipeline());
//spider.ThreadNumber = 10;
spider.Run();
~~~

~~~ c#
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
~~~

## 使用redis作为队列程序

安装包 [DotnetSpiderLite.Scheduler.StackExchange.Redis](https://www.nuget.org/packages/DotnetSpiderLite.Scheduler.StackExchange.Redis/)， 这个默认为使用 [StackExchange.Redis](https://www.nuget.org/packages/StackExchange.Redis/) 组件。

~~~ csharp
Spider spider = Spider.Create("https://www.cnblogs.com/");
// ...
spider.UseRedisScheduler("localhost");
// ...
spider.Run();
~~~

如果需要使用其他 redis 组件，可安装包 [DotnetSpiderLite.Scheduler.Redis](https://www.nuget.org/packages/DotnetSpiderLite.Scheduler.Redis/) , 然后实现 IRedisStore 接口。

~~~ csharp
Spider spider = Spider.Create("https://www.cnblogs.com/");
// ...

IRedisStore myRedisStore = [YOU REDISSTORE];
spider.UseRedisScheduler(myRedisStore);

// ...
spider.Run();
~~~