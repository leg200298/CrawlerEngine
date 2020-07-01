using CrawlerEngine.Crawler.Interface;
using CrawlerEngine.Crawler.WorkClass;
using CrawlerEngine.Models;
using System;

namespace CrawlerEngine.Crawler
{
    public class CrawlerFactory
    {
        public ICrawler GetCrawler(JobInfo jobInfo)
        {
            var target = jobInfo.Info["url"].ToString();


            switch (target.ToUpper())
            {
                case "MOMOCATEGORY":
                    return new WebCrawler();
                case "PCHOMEDETAIL":
                default:
                    return new HttpCrawler();
            }
            throw new Exception();

        }
    }
}
