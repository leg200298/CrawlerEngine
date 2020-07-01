using CrawlerEngine.Crawler;
using CrawlerEngine.Driver;
using CrawlerEngine.Driver.WorkClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrawlerEngine.Crawler.WorkClass
{
    class WebCrawler : CrawlerBase
    {
        private SeleniumDriver sd;
        protected override string GetData()
        {
            throw new NotImplementedException();
        }

        protected override void OpenUrl(string url)
        {
          sd =   WebDriverPool.GetFreeDriver();
            sd.Navigate().GoToUrl(url);
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
