using CrawlerEngine.Crawler.Interface;
using CrawlerEngine.Driver.WorkClass;
using System;

namespace CrawlerEngine.Crawler
{
    public abstract class CrawlerBase : ICrawler
    {
       
        /// <summary>
        /// 執行爬蟲 開啟網頁 並且取得資料( 睡秒數 重制 開啟網址 取得資料)
        /// </summary>
        /// <returns></returns>
        public abstract string DoCrawlerFlow();
       

    }
}
