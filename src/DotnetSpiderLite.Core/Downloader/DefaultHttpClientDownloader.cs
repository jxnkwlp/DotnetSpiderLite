using DotnetSpiderLite;
using DotnetSpiderLite.Downloader;
using DotnetSpiderLite.Logs;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpiderLite.Downloader
{
    /// <summary>
    ///  默认下载器，使用 <see cref="HttpClient"/> 
    /// </summary>
    public class DefaultHttpClientDownloader : BaseDownloader
    {
        private MyHttpClientHandler _myHttpClientHandler = new MyHttpClientHandler(null);
        private HttpClient _httpClient;
        private IWebProxy _lastWebProxy = null;

        public DefaultHttpClientDownloader()
        {
            _httpClient = CreateHttpClient();
        }

        public override IDownloader Clone()
        {
            return MemberwiseClone() as IDownloader;
        }

        public override void Dispose()
        {
            base.Dispose();

            _httpClient.Dispose();
        }

        HttpClient CreateHttpClient()
        {
            _myHttpClientHandler = new MyHttpClientHandler(Proxy);
            var httpClient = new HttpClient(_myHttpClientHandler)
            {
                Timeout = TimeSpan.FromSeconds(15)  // 15s
            };
            return httpClient;
        }

        public override async Task<Response> HandleDownloadAsync(Request request)
        {
            if ((this.Proxy != null || _lastWebProxy != null) && _lastWebProxy != Proxy)
            {
                _httpClient = CreateHttpClient();
                _lastWebProxy = Proxy;
            }

            SetOptions(request);

            var requestMessage = new HttpRequestMessage();

            requestMessage.Method = (string.Equals(request.Method, "POST", StringComparison.OrdinalIgnoreCase)) ? HttpMethod.Post : HttpMethod.Get;

            requestMessage.RequestUri = request.Uri;

            if (Uri.TryCreate(request.Referer, UriKind.Absolute, out var refererUrl))
                requestMessage.Headers.Referrer = refererUrl;

            if (request.Body != null)
            {
                requestMessage.Content = new ByteArrayContent(request.Body);
            }

            if (requestMessage.Content != null)
            {
                requestMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(request.ContentType);
            }

            HttpResponseMessage responseMessage = null;

            try
            {
                responseMessage = await _httpClient.SendAsync(requestMessage);
            }
            catch (Exception ex)
            {
                throw new DownloaderException($"请求URL失败{request.Uri}", ex);
            }

            responseMessage.EnsureSuccessStatusCode();

            var response = new Response(request);

            response.StatusCode = (int)responseMessage.StatusCode;
            if (responseMessage.Content != null)
            {
                response.ContentType = responseMessage.Content.Headers.ContentType.ToString();
                response.ContentLength = responseMessage.Content.Headers.ContentLength.HasValue ? responseMessage.Content.Headers.ContentLength.Value : -1;
                response.Body = await responseMessage.Content.ReadAsByteArrayAsync();
            }

            response.ResponseUri = responseMessage.RequestMessage.RequestUri;

            if (_myHttpClientHandler != null)
                response.ResponseCookies = _myHttpClientHandler.CookieContainer.GetCookies(request.Uri);

            return response;
        }


        private void SetOptions(Request request)
        {
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(request.UserAgent);
            if (!string.IsNullOrEmpty(request.Referer))
            {
                _httpClient.DefaultRequestHeaders.Referrer = new Uri(request.Referer);
            }
            else
            {
                _httpClient.DefaultRequestHeaders.Referrer = null;
            }

            _httpClient.DefaultRequestHeaders.Accept.ParseAdd(request.Accept);
            // _httpClient.DefaultRequestHeaders.AcceptEncoding.ParseAdd(request.AcceptEncoding);
            _httpClient.DefaultRequestHeaders.AcceptLanguage.ParseAdd(request.AcceptLanguage);


            _httpClient.DefaultRequestHeaders.ConnectionClose = !request.KeepAlive;

            if (request.Headers != null && request.Headers.Count > 0)
            {
                foreach (var item in request.Headers)
                {
                    if (_httpClient.DefaultRequestHeaders.Contains(item.Key))
                        _httpClient.DefaultRequestHeaders.Remove(item.Key);

                    _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(item.Key, item.Value);
                }
            }

        }

    }

    class MyHttpClientHandler : HttpClientHandler
    {
        public MyHttpClientHandler(IWebProxy webProxy)
        {
            this.UseCookies = true;
            this.CookieContainer = DownloaderCookieContainer.Instance;

            this.UseProxy = webProxy != null;
            this.Proxy = webProxy;

            this.UseDefaultCredentials = true;
        }
    }
}
