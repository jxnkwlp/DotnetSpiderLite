using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpiderLite.Downloader
{
    /// <summary> 
    /// </summary>
    public class DefaultDownloader : BaseDownloader
    {
        public override IDownloader Clone()
        {
            return MemberwiseClone() as IDownloader;
        }

        public override async Task<Response> HandleDownloadAsync(Request request)
        {
            var webRequest = WebRequest.Create(request.Uri) as HttpWebRequest;
            webRequest.Method = request.Method;
            webRequest.Proxy = null;
            webRequest.Timeout = 60 * 1000;
            webRequest.UseDefaultCredentials = true;
            webRequest.Referer = request.Referer;
            webRequest.UserAgent = request.UserAgent;
            webRequest.KeepAlive = request.KeepAlive;
            webRequest.CookieContainer = DownloaderCookieContainer.Instance;


            if (string.Equals(request.Method, "post", StringComparison.InvariantCultureIgnoreCase))
            {
                if (request.Body != null)
                {
                    var requestStream = webRequest.GetRequestStream();
                    requestStream.Write(request.Body, 0, request.Body.Length);

                    webRequest.ContentLength = request.Body.Length;
                    webRequest.ContentType = request.ContentType;

                }
            }

            var webResponse = await webRequest.GetResponseAsync();
            var stream = webResponse.GetResponseStream();


            var result = new Response(request)
            {
                ResponseUri = webResponse.ResponseUri,
                Body = stream,
                ContentType = webResponse.ContentType,
                ContentLength = webResponse.ContentLength,
            };

            if (webResponse is HttpWebResponse httpWebResponse)
            {
                result.StatusCode = (int)httpWebResponse.StatusCode;

                DownloaderCookieContainer.Instance.Add(httpWebResponse.Cookies);

                result.ResponseCookies = httpWebResponse.Cookies;

            }

            return result;
        }
    }
}
