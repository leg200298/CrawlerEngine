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
            xPaths.Clear();
            foreach (var a in jobInfo.Info["xPathList"].ToList())
            {
                xPaths.Add(a.ToString());
            }
        }

        protected override string GetData()
        {
            var s = sd.FindElementByXPath("/html/body");
            var qq = sd.FindElementByXPath("//*[@id=\"ToothContainer\"]/div/ul[3]/li[1]/a").GetAttribute("innerHTML");
            return s.GetAttribute("innerHTML");

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
