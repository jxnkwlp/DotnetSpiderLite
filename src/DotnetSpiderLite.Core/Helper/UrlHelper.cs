using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Helper
{
    public static class UrlHelper
    {
        public static IDictionary<string, string> ParseQuery(string query)
        {
            var resullt = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(query))
                return resullt;

            var items = query.Split('&');
            foreach (var item in items)
            {
                var keys = item.Split('=');
                if (keys.Length == 2)
                    resullt[keys[0]] = keys[1];
                else
                    resullt[item] = item;
            }

            return resullt;
        }
    }
}
