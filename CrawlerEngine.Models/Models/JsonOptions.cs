using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace CrawlerEngine.Models.Models
{
    public class JsonOptions
    {


        /// <summary>
        /// 字典暫存檔
        /// </summary>
        public Dictionary<string, object> Info = new Dictionary<string, object>();
        public object price { set { PutToDic("price", value); } }
        public object name { set { PutToDic("name", value); } }
        public object category { set { PutToDic("category", value); } }


        /// <summary>
        /// 將Key value寫入字典暫存檔
        /// </summary>
        /// <param name="key">關鍵字</param>
        /// <param name="value">值</param>
        public void PutToDic(string key, object value)
        {
            Info.Add(key, value);
        }

        /// <summary>
        /// 取得轉換成Json字串(by 字典黨)
        /// </summary>
        /// <returns>Json字典黨</returns>
        public string GetJsonString()
        {
            return JObject.FromObject(Info).ToString();
        }

    }
}
