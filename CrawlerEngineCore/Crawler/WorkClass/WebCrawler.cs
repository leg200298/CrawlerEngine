using CrawlerEngine.Crawler.Interface;
using CrawlerEngine.Driver;
using CrawlerEngine.Models;

namespace CrawlerEngine.Crawler.WorkClass
{
    public class WebCrawler : ICrawler
    {
        private JobInfo jobInfo;

        private int driverId;
        public WebCrawler(JobInfo jobInfo)
        {
            this.jobInfo = jobInfo;
        }

        public string DoCrawlerFlow()
        {
            GetDriver();
            OpenUrl();
            return GetData();
        }

        protected string GetData()
        {
            string responseData = string.Empty;
            try
            {
                responseData = WebDriverPool.DriverPool[driverId].FindElementByXPath("/html/body").GetAttribute("innerHTML");
            }
            finally
            {
                WebDriverPool.DriverPool[driverId].Status = Common.Enums.ObjectStatus.Driver.FREE;
            }
            return responseData;
        }

        private void OpenUrl()
        {
            WebDriverPool.DriverPool[driverId].Navigate().GoToUrl(jobInfo.Url);
        }


        private void GetDriver()
        {
            driverId = WebDriverPool.GetFreeDriver();
            WebDriverPool.DriverPool[driverId].Status = Common.Enums.ObjectStatus.Driver.NOTFREE;

        }

    }
}
