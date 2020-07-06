using CrawlerEngine.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrawlerEngine.Models.Models
{
    public class JsonOptions
    {


        /// <summary>
        /// 字典暫存檔
        /// </summary>
        public Dictionary<string, object> Info = new Dictionary<string, object>();
        public object price { set { PutToDic("_productPrice", value); } }
        public object name { set { PutToDic("_productName", value); } }
        public object category { set { PutToDic("_productCategory", value); } }


        /// <summary>
        /// 將Key value寫入字典暫存檔
        /// </summary>
        /// <param name="key">關鍵字</param>
        /// <param name="value">值</param>
        public void PutToDic(string key, object value)
        {
            if (Info.Keys.Contains(key))
            {
                Info[key] = value;
            }
            else { Info.Add(key, value); }

        }

        /// <summary>
        /// 取得轉換成Json字串(by 字典黨)
        /// </summary>
        /// <returns>Json字典黨</returns>
        public string GetJsonString()
        {
            PutToDic("_saveDataTime", DateTime.UtcNow.ToString(RuleString.DateTimeFormat));
            return JObject.FromObject(Info).ToString();
        }

    }
}
