using CrawlerEngine.Crawler.Interface;
using CrawlerEngine.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrawlerEngine.Crawler
{
    class CrawlerFactory
    {
        public ICrawler GetCrawler(JobInfo jobInfo) {
            throw new Exception();
        }
    }
}
