using DotnetSpiderLite;
using DotnetSpiderLite.Downloader;
using DotnetSpiderLite.Logs;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpiderLite.Downloader
{
    public class DefaultHttpClientDownloader : BaseDownloader
    {
        private static MyHttpClientHandler _myHttpClientHandler = new MyHttpClientHandler();

        private static HttpClient _httpClient = new HttpClient(_myHttpClientHandler);

        static DefaultHttpClientDownloader()
        {
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

        public override async Task<Response> HandleDownloadAsync(Request request)
        {
            HttpResponseMessage responseMessage = null;

            SetOptions(request);

            if (request.Method == "GET")
            {
                responseMessage = await _httpClient.GetAsync(request.Uri);
            }
            else
            {
                if (request.Body != null)
                    responseMessage = await _httpClient.PostAsync(request.Uri, new ByteArrayContent(request.Body));
                else
                    responseMessage = await _httpClient.PostAsync(request.Uri, null);
            }

            if (responseMessage == null)
            {
                throw new DownloaderException();
            }

            this.Logger?.Trace("Download page Http Status : " + responseMessage.StatusCode);

            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new DownloaderException();
            }

            var response = new Response(request);

            response.StatusCode = (int)responseMessage.StatusCode;
            response.ContentType = responseMessage.Content.Headers.ContentType.ToString();

            response.Body = await responseMessage.Content.ReadAsStreamAsync();
            response.ResponseUri = responseMessage.RequestMessage.RequestUri;
            response.ContentLength = (await responseMessage.Content.ReadAsStringAsync()).Length;

            response.ResponseCookies = _myHttpClientHandler.CookieContainer.GetCookies(request.Uri); 

            return response;
        }


        private void SetOptions(Request request)
        {
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
        public MyHttpClientHandler()
        {
            this.UseCookies = true;
            this.CookieContainer = DownloaderCookieContainer.Instance;
        }
    }
}
