using DotnetSpiderLite.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotnetSpiderLite
{
    public static class RequesetExtensions
    {
        const string REQUEST_RETRY_COUNT = "REQUEST_RETRY_COUNT";

        public static string GetIdentity(this Request request)
        {
            int retryCount = request.GetRetryCount();
            var input = new
            {
                request.Method,
                request.Uri,
                request.Encoding,
                request.Headers,
                request.Referer,
                retryCount,
            };
            return string.Concat(MD5Helper.ComputeMD5(Encoding.UTF8.GetBytes(JsonHelper.Serialize(input))).Select(t => t.ToString("X2")));
        }

        public static int GetRetryCount(this Request request)
        {
            if (request.Extra.ContainsKey(REQUEST_RETRY_COUNT))
            {
                int.TryParse(request.Extra[REQUEST_RETRY_COUNT], out var count);
                return count;
            }
            else
            {
                return 0;
            }
        }

        public static void IncrementRetryCount(this Request request)
        {
            if (request.Extra.ContainsKey(REQUEST_RETRY_COUNT))
            {
                int.TryParse(request.Extra[REQUEST_RETRY_COUNT], out var count);
                request.Extra[REQUEST_RETRY_COUNT] = (count + 1).ToString();
            }
            else
            {
                request.Extra[REQUEST_RETRY_COUNT] = "1";
            }
        }
    }
}
