using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpiderLite.Downloader.WebDriver
{
    public class WebDriverDownloaderOptions
    {
        public static WebDriverDownloaderOptions Default = new WebDriverDownloaderOptions();


        public bool LoadImage { get; set; } = true;

        public bool AlwaysLoadNoFocusLibrary { get; set; } = true;

        public bool LoadFlashPlayer { get; set; } = true;

        public bool Headless { get; set; }

        public int WebDriverNavigateWaitInterval { get; set; } = 500;
    }
}
