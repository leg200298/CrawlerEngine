using CrawlerEngine.Common.Extansion;
using CrawlerEngine.Common.Helper;
using CrawlerEngine.Driver;
using CrawlerEngine.Models;
using CrawlerEngine.Repository.Factory;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Threading;
using static CrawlerEngine.Common.Enums.ElectronicBusiness;

namespace CrawlerEngine.JobWorker.WorkClass
{

    /// <summary>
    /// 館分類頁
    /// </summary>
    class Pchome24hStoreJobWorker : JobWorkerBase
    {
        private List<JobInfo> jobInfos = new List<JobInfo>();
        private HtmlDocument htmlDoc = new HtmlDocument();
        public Pchome24hStoreJobWorker(JobInfo jobInfo)
        {
            this.jobInfo = jobInfo;
        }
        public override JobInfo jobInfo { get; set; }


        private int driverId;

        protected override bool GotoNextPage(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }
            else
            {
                if (url.StartsWith("//24h.pchome.com.tw"))
                {
                    jobInfo.Url = $"https:{url}";
                }

                return true;
            }
        }

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

        protected override bool Parse()
        {
            responseData = responseData.Replace("<div class=\"prod_info\">", "");
            htmlDoc.LoadHtml(responseData);
            var nodes = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"ProdGridContainer\"]/dd/h5/a");
            if (nodes is null) { return false; }
            foreach (var data in nodes)
            {
                var url = data.Attributes["href"].Value;
                if (url.StartsWith("//24h.pchome.com.tw"))
                {
                    jobInfos.Add(new JobInfo() { JobType = Platform.Pchome24hProduct.GetDescription(), Url = $"https:{url}" });
                }
            }
            return true;
        }

        protected override bool SaveData(CrawlFactory crawlFactory)
        {

            foreach (var d in jobInfos)
            {
                crawlFactory.CrawlDataJobListRepository.InsertOne(d, Platform.Pchome24hProduct.GetDescription());
            }
            jobInfos.Clear();
            return true;

        }

        protected override (bool, string) HasNextPage()
        {

            htmlDoc.LoadHtml(responseData);

            var nodes = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"PaginationContainer\"]/ul/li/a");
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    if (node.InnerText == "下一頁")
                    {
                        return (true, node.Attributes["href"].Value);
                    }
                }
            }
            return (false, "");
        }

        protected override void SleepForAWhile(decimal sleepTime)
        {
            Thread.Sleep((int)(sleepTime * 1000));
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
