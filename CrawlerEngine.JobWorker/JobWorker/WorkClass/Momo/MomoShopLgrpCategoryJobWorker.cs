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
using System.Text.RegularExpressions;
using System.Threading;
using static CrawlerEngine.Common.Enums.ElectronicBusiness;

namespace CrawlerEngine.JobWorker.WorkClass
{
    public class MomoShopLgrpCategoryJobWorker : JobWorkerBase
    {
        public override Logger _logger { get => LogManager.GetCurrentClassLogger(); }
        public override JobInfo jobInfo { get; set; }

        private List<JobInfo> jobInfos = new List<JobInfo>();
        private HtmlDocument htmlDoc = new HtmlDocument();

        private int driverId;
        public MomoShopLgrpCategoryJobWorker(JobInfo jobInfo)
        {
            this.jobInfo = jobInfo;
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

        protected override bool Crawl()
        {
            try
            {
                GetDriver();
                OpenUrl();
                responseData = GetData();
                return true;
            }
            catch (Exception ex)
            {
                LoggerHelper._.Error(ex);
                return false;
            }
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
            try
            {
                htmlDoc.LoadHtml(responseData);
                var nodes = htmlDoc.DocumentNode.SelectNodes("//*[@id='bt_category_Content']//a[contains(@href, 'category') and contains(@href, 'd_code')]");
                if (nodes is null) { return false; }
                foreach (var data in nodes)
                {
                    string href = HtmlEntity.DeEntitize(data.Attributes["href"].Value);
                    JobInfo jobInfo = new JobInfo()
                    {
                        Seq = Guid.NewGuid(),
                        JobType = Platform.MomoShopDgrpCategory.GetDescription(),
                        Url = href.StartsWith("https://www.momoshop.com.tw") ? href : $"https://www.momoshop.com.tw{href}"
                    };

                    Int64.TryParse(Regex.Match(data.Attributes["href"].Value, @"(d_code=\d+)").Value.Split('=')
                        .Where(x => Regex.IsMatch(x, @"\d+")).FirstOrDefault(), out Int64 cateCode);

                    string postData = "data=" + Uri.EscapeDataString(
                        $"{{\"flag\":2035,\"data\":{{\"params\":{{\"cateCode\":\"{cateCode}\",\"cateLevel\":\"3\",\"curPage\":\"1\"}}}}}}");

                    jobInfo.PutToDic("_apiUrl", "https://www.momoshop.com.tw/ajax/ajaxTool.jsp?n=2035");
                    jobInfo.PutToDic("_postData", postData);
                    jobInfos.Add(jobInfo);
                }
                return true;
            }
            catch (Exception ex)
            {
                LoggerHelper._.Error(ex);
                return false;
            }
        }

        protected override bool SaveData(CrawlFactory crawlFactory)
        {
            try
            {
                foreach (var job in jobInfos)
                {
                    crawlFactory.CrawlDataJobListRepository.InsertOne(job, Platform.MomoShopDgrpCategory.GetDescription());
                }
                return true;
            }
            catch (Exception ex)
            {
                LoggerHelper._.Error(ex);
                return false;
            }
        }

        protected override (bool, string) HasNextPage()
        {
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
