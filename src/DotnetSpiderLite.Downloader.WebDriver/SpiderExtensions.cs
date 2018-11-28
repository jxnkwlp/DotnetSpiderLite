using DotnetSpiderLite.Downloader.WebDriver;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace DotnetSpiderLite.Downloader
{
    public static class SpiderExtensions
    {
        public static Spider UseWebDriverDownloader(this Spider spider, RemoteWebDriver webDriver)
        {
            return spider.SetDownloader(new WebDriverDownloader(webDriver, WebDriverDownloaderOptions.Default));
        }

        public static Spider UseChromeWebDriverDownloader(this Spider spider, string binaryLocation)
        {
            // TODO  ChromeOptions 参数？


            var driver = new ChromeDriver(binaryLocation, new ChromeOptions()
            {

            });
            return spider.SetDownloader(new WebDriverDownloader(driver, WebDriverDownloaderOptions.Default));
        }
    }
}
