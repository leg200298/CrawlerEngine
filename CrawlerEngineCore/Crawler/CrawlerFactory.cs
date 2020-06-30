using CrawlerEngine.Crawler.Interface;
using CrawlerEngine.Crawler.WorkClass;
using CrawlerEngine.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrawlerEngine.Crawler
{
    public class CrawlerFactory
    {
        public ICrawler GetCrawler(JobInfo jobInfo) {
            //switch (jobInfo.TargetType) {
            //    case 2:
            //        return new WebCrawler();
            //    case 1:
            //    default:
            //        return new HttpCrawler();
            //}
            throw new Exception();

        }
    }
}
