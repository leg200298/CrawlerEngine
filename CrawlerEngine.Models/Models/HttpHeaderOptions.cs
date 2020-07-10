using System.Collections.Generic;
using System.Linq;

namespace CrawlerEngine.Models
{
    public class HttpHeaderOptions
    {

        /// <summary>
        /// 工作所需內容轉入存放字典
        /// </summary>
        public Dictionary<string, string> HeaderDic = new Dictionary<string, string>();


        /// <summary>
        /// 塞入值寫進字典
        /// </summary>
        /// <param name="key">要輸入的關鍵字</param>
        /// <param name="value">值</param>
        public void PutToHeaderDic(string key, string value)
        {
            HeaderDic[key] = value;
        }

        /// <summary>
        /// 取得的一種通用方法
        /// </summary>
        /// <param name="key">關鍵字</param>
        /// <returns>值</returns>
        public object GetFromHeaderDic(string key)
        {
            if (!HeaderDic.Keys.Contains(key))
            {
                return null;
            }
            return HeaderDic[key];
        }

        /// <summary>
        /// 取得的一種通用方法
        /// </summary>
        /// <param name="key">關鍵字</param>
        /// <returns>值</returns>
        public object DeleteFromHeaderDic(string key)
        {
            if (!HeaderDic.Keys.Contains(key))
            {
                return null;
            }
            HeaderDic.Remove(key);
            return true;
        }

    }
}
