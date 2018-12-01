//using DotnetSpiderLite.PageProcessor;
//using System;
//using System.Collections.Generic;
//using System.Net;
//using System.Text;

//namespace DotnetSpiderLite.NodeHub
//{
//    public class NodeHubSpider
//    {
//        public const int DEFAULTPORT = 10030;

//        private int _httpPort = DEFAULTPORT;
//        private HttpListener _listener = new HttpListener();
//        private Spider _spider;

//        public NodeHubSpider(Spider spider, int port)
//        {
//            _spider = spider;
//            _spider.OnHandleRequestDownload = OnSpiderHandleRequestDownload;

//            _httpPort = port;

//            Init();
//        }

//        public NodeHubSpider(Spider spider) : this(spider, DEFAULTPORT)
//        {

//        }

//        private void Init()
//        {
//            if (!HttpListener.IsSupported)
//            {
//                throw new PlatformNotSupportedException("当前系统不支持使用 HttpListener ");
//            }



//            var localIpaddress = Dns.GetHostAddresses(Dns.GetHostName());

//            // 对本机的IP 侦听

//            foreach (var iPAddress in localIpaddress)
//            {
//                // TODO 


//                var ip = $"http://{iPAddress.ToString()}:{_httpPort}/";

//                _listener.Prefixes.Add(ip);
//            }


//        }


//        private Response OnSpiderHandleRequestDownload(Request request)
//        {

//            return null;
//        }





//    }
//}
