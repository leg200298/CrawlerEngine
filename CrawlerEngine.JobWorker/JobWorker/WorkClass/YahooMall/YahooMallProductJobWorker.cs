using CrawlerEngine.Common.Helper;
using CrawlerEngine.Driver;
using CrawlerEngine.Model.DTO;
using CrawlerEngine.Models;
using CrawlerEngine.Repository.Factory;
using HtmlAgilityPack;
using NLog;
using System;
using System.Linq;
using System.Net.Http;

namespace CrawlerEngine.JobWorker.WorkClass
{
    /// <summary>
    /// 商品細節頁
    /// </summary>
    public class YahooMallProductJobWorker : JobWorkerBase
    {
        public YahooMallProductJobWorker(JobInfo jobInfo)
        {
            this.jobInfo = jobInfo;
        }
        public override Logger _logger { get => LogManager.GetCurrentClassLogger(); }
        public override JobInfo jobInfo { get; set; }

        private int driverId;
        protected override bool Crawl()
        {
            var success = false;
            try
            {
                var httpClient = new HttpClient();
                var httpResponse = httpClient.GetAsync(jobInfo.Url).GetAwaiter().GetResult();
                responseData = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                success = true;
            }
            catch (Exception ex)
            {
                LoggerHelper._.Error(ex);
            }
            return success;
        }
        protected override bool GotoNextPage(string url)
        {
            return false;
        }

        protected override (bool, string) HasNextPage()
        {

            return (false, "");
        }

        protected override bool Parse()
        {

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(responseData);//*[@id="yui_3_12_0_2_1594632086640_34"]/div[4]/div[1]/h1/span[1]
            var qq = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"ypsiif\"]/div/div//li[@class='tumblr']/a");
            var qq222 = qq.InnerText;
          var qqwww=  qq.GetAttributes("data-caption").FirstOrDefault().Value;
            htmlDoc.LoadHtml(qqwww);
            var qq2 = htmlDoc.DocumentNode.SelectSingleNode("/a").InnerText;
           var outer= htmlDoc.DocumentNode.InnerText;
            crawlDataDetailOptions.price = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"ypsiif\"]/table/tbody/tr[1]/td/div").InnerText;
            crawlDataDetailOptions.price = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"ypsiif\"]/table/tbody/tr[1]/td/div/").InnerText;
            crawlDataDetailOptions.price = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"ypsiif\"]/table/tbody/tr[1]/td/div/span").InnerText;
            crawlDataDetailOptions.name = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"ypsiif\"]/div/div[1]/div[4]/div[1]/h1/span[1]").InnerText;
            crawlDataDetailOptions.category = "";
            return true;
        }

        protected override bool SaveData(CrawlFactory crawlFactory)
        {
            CrawlDataDetailDto crawlDataDetailDto = new CrawlDataDetailDto()
            {
                seq = jobInfo.Seq,
                job_status = "end",
                end_time = DateTime.UtcNow,
                detail_data = crawlDataDetailOptions.GetJsonString()
            };

            crawlFactory.CrawlDataDetailRepository.InsertDataDetail(crawlDataDetailDto);
            return true;

        }

        protected override void SleepForAWhile(decimal sleepTime)
        {

        }


        protected override bool Validate()
        {
            if (string.IsNullOrEmpty(responseData))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #region WebBrowser

        private void GetDriver()
        {

            driverId = WebDriverPool.GetFreeDriver();

            WebDriverPool.DriverPool[driverId].Status = Common.Enums.ObjectStatus.Driver.NOTFREE;

        }
        private void OpenUrl()
        {
            WebDriverPool.DriverPool[driverId].ChromeDriver.Navigate().GoToUrl(jobInfo.Url);
        }



        protected string GetData()
        {
            string responseData = string.Empty;
            try
            {
                WebDriverPool.DriverPool[driverId].ChromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                responseData = WebDriverPool.DriverPool[driverId].ChromeDriver.FindElementByXPath("/html/body").GetAttribute("innerHTML");
                ScrollMove();

            }
            catch (Exception ex)
            {
                LoggerHelper._.Error(ex);
            }
            finally
            {
                WebDriverPool.DriverPool[driverId].Status = Common.Enums.ObjectStatus.Driver.FREE;
            }
            return responseData;
        }


        private void ScrollMove()
        {
            OpenQA.Selenium.IJavaScriptExecutor jse = WebDriverPool.DriverPool[driverId].ChromeDriver;
            int height = (int)Math.Ceiling(1000 * 0.1);
            jse.ExecuteScript("window.scrollBy(0," + height + ")");
        }

        #endregion
    }
}
