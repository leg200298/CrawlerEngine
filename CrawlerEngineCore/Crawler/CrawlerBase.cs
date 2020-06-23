using CrawlerEngine.Crawler.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrawlerEngine.Crawler
{
    abstract class CrawlerBase : ICrawler
    {
        public DateTime StartTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        protected string url { get; set; }

        public string DoCrawlerFlow()
        {
            OpenUrl(url);
            return GetData();
        }

        protected abstract void OpenUrl(string url);

        protected abstract string GetData();
    }
}
