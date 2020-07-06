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

        protected override string GetData()
        {
            HttpClient hc = new HttpClient();
            var httpResponse = hc.GetAsync(jobInfo.Url).GetAwaiter().GetResult();
            return httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        }

        protected override void GetDriver()
        {

        }

        protected override void OpenUrl(string url)
        {
        }

        protected override void Reset()
        {
        }

        protected override void Sleep(int time)
        {
        }
    }
}
