using DotnetSpiderLite.Downloader.WebDriver;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.IO;
using System.Reflection;

namespace DotnetSpiderLite.Downloader
{
    public static class SpiderExtensions
    {
        public static Spider UseChromeWebDriverDownloader(this Spider spider)
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            return spider.UseChromeWebDriverDownloader(baseDirectory);
        }
    }
}
