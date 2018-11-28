using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotnetSpiderLite.Downloader.WebDriver
{
    public class WebDriverDownloader : BaseDownloader
    {
        private RemoteWebDriver _driver;
        private WebDriverDownloaderOptions _options;

        public WebDriverDownloader(RemoteWebDriver webDriver, WebDriverDownloaderOptions options)
        {
            _driver = webDriver;
            _options = options;


        }

        public override IDownloader Clone()
        {
            return this.MemberwiseClone() as IDownloader;
        }

        public override Task<Response> HandleDownloadAsync(Request request)
        {
            // TODO Request  属性写入 和 Response 属性写入

            var cookies = request.CookieContainer.GetCookies(request.Uri);

            foreach (Cookie cookie in cookies)
            {
                _driver.Manage().Cookies.AddCookie(new Cookie(cookie.Name, cookie.Value, cookie.Domain, cookie.Path, cookie.Expiry));
            }

            Logger?.Log(Logs.LogLevel.Trace, "浏览器加载中...");

            _driver.Navigate().GoToUrl(request.Uri);

            if (_options.WebDriverNavigateWaitInterval > 0)
                Thread.Sleep(_options.WebDriverNavigateWaitInterval);

            var response = new Response(request);

            string html = _driver.PageSource;

            if (!string.IsNullOrEmpty(html))
                response.Body = request.Encoding.GetBytes(html);


            return Task.FromResult(response);

        }


    }
}
