using CrawlerEngine.Crawler.Interface;
using CrawlerEngine.Driver.WorkClass;
using System;

namespace CrawlerEngine.Crawler
{
    public abstract class CrawlerBase : ICrawler
    {
        private int time;

        protected SeleniumDriver sd;
        public DateTime StartTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        protected string url { get; set; }

        /// <summary>
        /// 執行爬蟲 開啟網頁 並且取得資料( 睡秒數 重制 開啟網址 取得資料)
        /// </summary>
        /// <returns></returns>
        public string DoCrawlerFlow()
        {
            GetDriver();
            Sleep(time);
            Reset();
            OpenUrl(url);
            var returnData = GetData();
            sd.Status = Common.NamingString.ObjectStatus.DriverStatus.FREE;
            return returnData;
        }
        protected abstract void GetDriver();
        protected abstract void OpenUrl(string url);

        protected abstract string GetData();
        protected abstract void Reset();
        protected abstract void Sleep(int time);
    }
}
