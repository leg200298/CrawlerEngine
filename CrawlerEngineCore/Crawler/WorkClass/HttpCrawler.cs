using CrawlerEngine.Models;
using System;

namespace CrawlerEngine.Crawler.WorkClass
{
    class HttpCrawler : CrawlerBase
    {
        private JobInfo jobInfo;

        public HttpCrawler(JobInfo jobInfo)
        {
            this.jobInfo = jobInfo;

            url = jobInfo.Info["url"].ToString();
        }

        protected override string GetData()
        {
            throw new NotImplementedException();
        }

        protected override void GetDriver()
        {

        }

        protected override void OpenUrl(string url)
        {
            throw new NotImplementedException();
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
