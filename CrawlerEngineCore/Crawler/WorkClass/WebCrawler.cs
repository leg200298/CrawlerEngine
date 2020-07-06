using CrawlerEngine.Driver;
using CrawlerEngine.Models;

namespace CrawlerEngine.Crawler.WorkClass
{
    public class WebCrawler : CrawlerBase
    {
        private JobInfo jobInfo;

        private int driverId;
        public WebCrawler(JobInfo jobInfo)
        {
            this.jobInfo = jobInfo;
        }

        protected override string GetData()
        {
            var responseData= WebDriverPool.DriverPool[driverId].FindElementByXPath("/html/body").GetAttribute("innerHTML");
            WebDriverPool.DriverPool[driverId].Status = Common.NamingString.ObjectStatus.DriverStatus.FREE;
            return responseData;
        }

        protected override void OpenUrl(string url)
        {
            url = jobInfo.Url;
            WebDriverPool.DriverPool[driverId].Navigate().GoToUrl(url);
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
            driverId = WebDriverPool.GetFreeDriver();
            WebDriverPool.DriverPool[driverId].Status = Common.NamingString.ObjectStatus.DriverStatus.NOTFREE;
        }

    }
}
