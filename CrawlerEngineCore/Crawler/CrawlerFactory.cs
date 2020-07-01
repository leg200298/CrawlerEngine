using CrawlerEngine.Crawler.Interface;
using CrawlerEngine.Crawler.WorkClass;
using CrawlerEngine.Models;

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
                    return new HttpCrawler(jobInfo);
                case "PCHOMEDETAIL":
                default:
                    return new WebCrawler(jobInfo);
            }

        }
    }
}
