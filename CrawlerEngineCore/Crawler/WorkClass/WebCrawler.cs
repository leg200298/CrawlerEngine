using CrawlerEngine.Driver;
using CrawlerEngine.Models;

namespace CrawlerEngine.Crawler.WorkClass
{
    public class WebCrawler : CrawlerBase
    {
        private JobInfo jobInfo;

        public WebCrawler(JobInfo jobInfo)
        {
            this.jobInfo = jobInfo;
        }

        protected override string GetData()
        {
            var c = sd.FindElementById("ProdGridContainer");
            var c2 = sd.FindElementByXPath("//*[@id=\"ProdGridContainer\"]/dd");
            return sd.FindElementByXPath("/html/body").GetAttribute("innerHTML");
        }

        protected override void OpenUrl(string url)
        {
            url = jobInfo.Url;
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
