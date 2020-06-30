using CrawlerEngine.Crawler;
using CrawlerEngine.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrawlerEngine.Crawler.WorkClass
{
    class WebCrawler : CrawlerBase
    {
        protected override string GetData()
        {
            throw new NotImplementedException();
        }

        protected override void OpenUrl(string url)
        {
            WebDriverPool.GetFreeDriver();
        }

        protected override void Reset()
        {
            throw new NotImplementedException();
        }

        protected override void Sleep(int time)
        {
            throw new NotImplementedException();
        }
    }
}
