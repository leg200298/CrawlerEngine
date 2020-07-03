using CrawlerEngine.Crawler.Interface;
using CrawlerEngine.Crawler.WorkClass;
using CrawlerEngine.Models;

namespace CrawlerEngine.Crawler
{
    public class CrawlerFactory
    {
        public ICrawler GetCrawler(JobInfo jobInfo)
        {
            var target = jobInfo.JobType;


            switch (target.ToUpper())
            {
                case "MOMO-PRODUCT":
                    return new HttpCrawler(jobInfo);
                case "PCHOME-PRODUCT":
                default:
                    return new WebCrawler(jobInfo);
            }

        }
    }
}
