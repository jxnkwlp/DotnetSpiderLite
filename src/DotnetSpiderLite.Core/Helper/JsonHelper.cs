using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Helper
{
    static class JsonHelper
    {
        public static string Serialize<T>(T obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        public static T Deserialize<T>(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }
    }
}
