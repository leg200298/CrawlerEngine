using CrawlerEngine.Driver;
using CrawlerEngine.Models;

namespace CrawlerEngine.Crawler.WorkClass
{
    class WebCrawler : CrawlerBase
    {
        private JobInfo jobInfo;

        public WebCrawler(JobInfo jobInfo)
        {
            this.jobInfo = jobInfo;
            url = jobInfo.Url;
        }

        protected override string GetData()
        {
            return sd.FindElementByXPath("/html/body").GetAttribute("innerHTML");
        }

        protected override void OpenUrl(string url)
        {
            sd.Navigate().GoToUrl(url);
        }

        protected override void Reset()
        {
        }

        protected override void Sleep(int time)
        {
            System.Threading.Thread.Sleep(time * 1000);
        }

        protected override void GetDriver()
        {
            sd = WebDriverPool.GetFreeDriver();
        }
    }
}
