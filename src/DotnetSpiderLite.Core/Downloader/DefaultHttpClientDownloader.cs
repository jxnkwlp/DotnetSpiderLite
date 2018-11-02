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

        public override async Task DownloadHandleAsync(DownloadContext context)
        {
            HttpResponseMessage responseMessage = null;

            if (context.Request.Method == "GET")
            {
                responseMessage = await _httpClient.GetAsync(context.Request.Uri);
            }
            else
            {
                if (context.Request.Body != null)
                    responseMessage = await _httpClient.PostAsync(context.Request.Uri, new StreamContent(context.Request.Body));
                else
                    responseMessage = await _httpClient.PostAsync(context.Request.Uri, null);
            }



            context.Response.StatusCode = (int)responseMessage.StatusCode;
            context.Response.ContentType = responseMessage.Content.Headers.ContentType.ToString();

            context.Response.Body = await responseMessage.Content.ReadAsStreamAsync();

        }

    }
}
