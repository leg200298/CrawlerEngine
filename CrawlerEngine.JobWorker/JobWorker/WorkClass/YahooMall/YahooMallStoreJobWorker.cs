
using CrawlerEngine.Common.Extansion;
using CrawlerEngine.Common.Helper;
using CrawlerEngine.Driver;
using CrawlerEngine.Models;
using CrawlerEngine.Repository.Factory;
using HtmlAgilityPack;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrawlerEngine.JobWorker.WorkClass
{
    /// <summary>
    /// 商品細節頁
    /// </summary>
    public class YahooMallStoreJobWorker : JobWorkerBase
    {
        public YahooMallStoreJobWorker(JobInfo jobInfo)
        {
            this.jobInfo = jobInfo;

        }
        public override Logger _logger { get => LogManager.GetCurrentClassLogger(); }
        private List<JobInfo> jobInfos = new List<JobInfo>();
        private HtmlDocument htmlDoc = new HtmlDocument();
        public override JobInfo jobInfo { get; set; }
        private int driverId;
        protected override bool Crawl()
        {
            var success = false;
            try
            {
                GetDriver();
                OpenUrl();
                responseData = GetData();
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
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }
            else
            {

                jobInfo.Url = url;

                return true;
            }
        }

        protected override (bool, string) HasNextPage()
        {


            htmlDoc.LoadHtml(responseData);
            //   node = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"ypsaupg\"]/div[2]/a");
            var nodes = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"ypsaupg\"]/div/a");

            var node = nodes.Where(x => x.InnerText.Contains("下一頁")).FirstOrDefault();
            if (node != null)
            {
                var url = new Uri(new Uri(jobInfo.Url), node.Attributes["href"].Value.Replace("&amp;", "&")).ToString();

                return (true, url);
            }
            else
            {
                return (false, "");
            }

        }
        protected override bool Parse()
        {

            htmlDoc.LoadHtml(responseData);
            var nodes = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"ypsausi\"]/div[2]/ul/li/div[1]/table/tbody/tr/td/a");
            if (nodes is null) { return false; }
            foreach (var data in nodes)
            {
                var url = data.Attributes["href"].Value;
                if (url.StartsWith("https://tw.mall.yahoo.com/item"))
                {
                    jobInfos.Add(new JobInfo() { JobType = Common.Enums.ElectronicBusiness.Platform.YahooMallProduct.GetDescription(), Url = $"{url}" });
                }
            }
            return true;
        }

        protected override bool SaveData(CrawlFactory crawlFactory)
        {
            foreach (var d in jobInfos)
            {
                crawlFactory.CrawlDataJobListRepository.InsertOne(d, Common.Enums.ElectronicBusiness.Platform.YahooMallProduct.GetDescription());
            }
            jobInfos.Clear();
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
