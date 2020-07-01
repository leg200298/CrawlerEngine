using CrawlerEngine.Driver;
using CrawlerEngine.Models;
using System.Linq;

namespace CrawlerEngine.Crawler.WorkClass
{
    class WebCrawler : CrawlerBase
    {
        private JobInfo jobInfo;

        public WebCrawler(JobInfo jobInfo)
        {
            this.jobInfo = jobInfo;
            url = jobInfo.Info["url"].ToString();
            xPaths = jobInfo.Info["xPathList"].ToString().Split(',').ToList();
        }

        protected override string GetData()
        {
            var s = sd.FindElementsByCssSelector("/html/body");
            return s[0].GetAttribute("innerHTML");

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
