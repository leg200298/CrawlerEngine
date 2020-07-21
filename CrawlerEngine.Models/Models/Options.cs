using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrawlerEngine.Models
{
    public class Options 
    {

        /// <summary>
        /// 工作所需內容轉入存放字典
        /// </summary>
        public Dictionary<string, object> Info = new Dictionary<string, object>();

        /// <summary>
        /// 透過關鍵字取值(字串)
        /// </summary>
        /// <param name="key">關鍵字</param>
        /// <returns></returns>
        protected string GetString(string key)
        {

            if (!Info.Keys.Contains(key))
            {
                return null;
            }
            return Info[key] as string;
        }

        /// <summary>
        /// 塞入值寫進字典
        /// </summary>
        /// <param name="key">要輸入的關鍵字</param>
        /// <param name="value">值</param>
        public void PutToDic(string key, object value)
        {
            Info[key] = value;
        }

        /// <summary>
        /// 取得的一種通用方法
        /// </summary>
        /// <param name="key">關鍵字</param>
        /// <returns>值</returns>
        public object GetFromDic(string key)
        {
            if (!Info.Keys.Contains(key))
            {
                return null;
            }
            return Info[key];
        }
        public string GetJsonString()
        {
            PutToDic("_saveDataTime", DateTime.UtcNow.ToString("yyyy/MM/dd hh:mm:ss"));
            return JObject.FromObject(Info).ToString();
        }


    }
}
