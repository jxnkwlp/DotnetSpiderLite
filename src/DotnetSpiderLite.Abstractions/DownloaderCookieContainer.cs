using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace DotnetSpiderLite
{
    public class DownloaderCookieContainer : CookieContainer
    {
        static DownloaderCookieContainer _instance = new DownloaderCookieContainer();

        public static DownloaderCookieContainer Instance { get => _instance; }

    }
}
