using DotnetSpiderLite.Abstractions;
using DotnetSpiderLite.Abstractions.Downloader;
using DotnetSpiderLite.Abstractions.Logs;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpiderLite.Core.Downloader
{
    public class DefaultHttpClientDownloader : BaseDownloader
    {
        private static HttpClient _httpClient = new HttpClient();

        public override async Task<Response> HandleDownloadAsync(Request request)
        {
            HttpResponseMessage responseMessage = null;

            if (request.Method == "GET")
            {
                responseMessage = await _httpClient.GetAsync(request.Uri);
            }
            else
            {
                if (request.Body != null)
                    responseMessage = await _httpClient.PostAsync(request.Uri, new StreamContent(request.Body));
                else
                    responseMessage = await _httpClient.PostAsync(request.Uri, null);
            }

            var response = new Response(request);

            response.StatusCode = (int)responseMessage.StatusCode;
            response.ContentType = responseMessage.Content.Headers.ContentType.ToString();

            response.Body = await responseMessage.Content.ReadAsStreamAsync();


            return response;
        }
    }
}
