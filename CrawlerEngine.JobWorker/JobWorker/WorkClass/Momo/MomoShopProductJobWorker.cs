using CrawlerEngine.Common.Helper;
using CrawlerEngine.Driver;
using CrawlerEngine.Model.DTO;
using CrawlerEngine.Models;
using HtmlAgilityPack;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace CrawlerEngine.JobWorker.WorkClass
{
    public class MomoShopProductJobWorker : JobWorkerBase
    {
        private HtmlDocument htmlDoc = new HtmlDocument();
        public MomoShopProductJobWorker(JobInfo jobInfo)
        {
            this.jobInfo = jobInfo;
        }
        private int driverId;
        public override JobInfo jobInfo { get; set; }

        protected override bool GotoNextPage(string url)
        {
            return false;
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

                //crawlDataDetailOptions.price = htmlDoc.DocumentNode.SelectSingleNode("//*[@class='priceTxtArea']//b").InnerText;
                //crawlDataDetailOptions.name = htmlDoc.DocumentNode.SelectSingleNode("//*[@id='goodsName']").InnerText;
                //crawlDataDetailOptions.category = string.Join(@">"
                //        , htmlDoc.DocumentNode.SelectNodes("//*[@class='pathArea']//a").Select(x => x.InnerText));

                crawlDataDetailOptions.price = string.Join("\\"
                    , htmlDoc.DocumentNode.SelectNodes("//*[@class='prdnoteArea']//*[contains(@class, 'prdPrice')]//li")
                    .Where(x => Regex.IsMatch(x.InnerText, @"\D+(價|價格)+(\d{1,3},)*\d+元"))
                    .Select(x => Regex.Match(x.InnerText, @"\D+(價|價格)+(\d{1,3},)*\d+元").Value?
                        .Replace(System.Environment.NewLine, string.Empty).Trim()));

                crawlDataDetailOptions.name = htmlDoc.DocumentNode.SelectSingleNode("//*[@class=\"prdnoteArea\"]//*[self::h1 or self::h2 or self::h3 or self::h4 or self::h5 or self::h6]").InnerText;
                crawlDataDetailOptions.category = string.Join("\\", htmlDoc.DocumentNode.SelectNodes("//*[@id=\"bt_2_layout_NAV\"]/ul//li").Select(x => x.InnerText));
                crawlDataDetailOptions.PutToDic("_img", $"https:{htmlDoc.DocumentNode.SelectSingleNode("//*[@id='goodsimgB']//a[contains(@href, 'goodsimg')]")?.Attributes["href"].Value}");

                return true;
            }
            catch (Exception ex)
            {
                LoggerHelper._.Error(ex);
                return false;
            }
        }

        protected override bool SaveData()
        {
            try
            {
                CrawlDataDetailDto crawlDataDetailDto = new CrawlDataDetailDto()
                {
                    Seq = jobInfo.Seq,
                    JobStatus = "end",
                    EndTime = DateTime.Now,
                    DetailData = crawlDataDetailOptions.GetJsonString()
                };

                Repository.Factory.CrawlFactory.CrawlDataDetailRepository.InsertDataDetail(crawlDataDetailDto);
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
