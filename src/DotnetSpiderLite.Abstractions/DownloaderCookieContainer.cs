using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace DotnetSpiderLite
{
    /// <summary>
    ///  默认cookie 容器
    /// </summary>
    public class DownloaderCookieContainer : CookieContainer
    {
        private static readonly DownloaderCookieContainer _instance = new DownloaderCookieContainer();

        public static DownloaderCookieContainer Instance { get => _instance; }

    }
}
