using CrawlerEngine.Models;
using System.Net.Http;

namespace CrawlerEngine.Crawler.WorkClass
{
    public class HttpCrawler : CrawlerBase
    {
        private JobInfo jobInfo;

        public HttpCrawler(JobInfo jobInfo)
        {
            this.jobInfo = jobInfo;

        }

        public override string DoCrawlerFlow()
        {
            return GetData();

        }

        private  string GetData()
        {
            var httpResponse = new HttpClient().GetAsync(jobInfo.Url).GetAwaiter().GetResult();
            return httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        }


    }
}
