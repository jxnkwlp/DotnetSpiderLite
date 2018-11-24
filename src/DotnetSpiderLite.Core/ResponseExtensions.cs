using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DotnetSpiderLite
{
    public static class ResponseExtensions
    {
        public static string GetBodyAsString(this Response response)
        {
            if (response?.Body == null)
                return null;

            return response.Request.Encoding.GetString(response.Body);
        }

    }
}
