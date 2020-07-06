using CrawlerEngine.Crawler.Interface;
using CrawlerEngine.Models;
using System.Net.Http;

namespace CrawlerEngine.Crawler.WorkClass
{
    public class HttpCrawler : ICrawler
    {
        private JobInfo jobInfo;

        public HttpCrawler(JobInfo jobInfo)
        {
            this.jobInfo = jobInfo;

        }

        public string DoCrawlerFlow()
        {
            return GetData();

        }

        private string GetData()
        {
            var httpResponse = new HttpClient().GetAsync(jobInfo.Url).GetAwaiter().GetResult();
            return httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        }


    }
}
