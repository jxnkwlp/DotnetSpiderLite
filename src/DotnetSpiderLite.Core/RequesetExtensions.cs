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
                request.Encoding.EncodingName,
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

        public static string ToJsonStrng(this Request request)
        {
            var data = new
            {
                request.Referer,
                request.KeepAlive,
                request.AcceptLanguage,
                request.AcceptEncoding,
                request.Accept,
                request.UserAgent,
                request.ContentType,
                Encoding = request.Encoding.BodyName,
                request.Method,
                Url = request.Uri,
                request.Body,
                request.Headers,
                request.Extra,
            };

            return Newtonsoft.Json.JsonConvert.SerializeObject(data);
        }

        public static Request ToRequest(this string requestJson)
        {
            var d = new
            {
                Referer = "",
                KeepAlive = false,
                AcceptLanguage = "",
                AcceptEncoding = "",
                Accept = "",
                UserAgent = "",
                ContentType = "",
                Encoding = "",
                Method = "",
                Url = "",
                Body = default(byte[]),
                Headers = default(Dictionary<string, string>),
                Extra = default(Dictionary<string, string>),
            };

            var data = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(requestJson, d);

            var request = new Request(new Uri(data.Url))
            {
                Accept = data.Accept,
                KeepAlive = data.KeepAlive,
                AcceptLanguage = data.AcceptLanguage,
                AcceptEncoding = data.AcceptEncoding,
                UserAgent = data.UserAgent,
                ContentType = data.ContentType,
                Encoding = Encoding.GetEncoding(data.Encoding),
                Method = data.Method,
                Body = data.Body,
                Referer = data.Referer,
            };

            request.Headers.Marge(data.Headers);
            request.Extra.Marge(data.Extra);

            return request;
        }
    }
}
