using Newtonsoft.Json;
using System.Collections.Generic;

namespace CrawlerEngine.Common.Helper
{
    public class JsonUntityHelper
    {
        public static string SerializeDictionaryToJsonString<TKey, TValue>(Dictionary<TKey, TValue> dict)
        {
            if (dict.Count == 0)
            {
                return "";
            }

            string jsonStr = JsonConvert.SerializeObject(dict);
            return jsonStr;
        }

        public static Dictionary<TKey, TValue> DeserializeStringToDictionary<TKey, TValue>(string jsonStr)
        {
            if (string.IsNullOrEmpty(jsonStr))
            {
                return new Dictionary<TKey, TValue>();
            }

            Dictionary<TKey, TValue> jsonDict = JsonConvert.DeserializeObject<Dictionary<TKey, TValue>>(jsonStr);

            return jsonDict;

        }
    }
}
