using CrawlerEngine.Common;
using CrawlerEngine.Common.Helper;
using CrawlerEngine.Driver;
using CrawlerEngine.Model.DTO;
using CrawlerEngine.Models;
using CrawlerEngine.Repository.Factory;
using HtmlAgilityPack;
using NLog;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using static CrawlerEngine.Common.Enums.ElectronicBusiness;

namespace CrawlerEngine.JobWorker.WorkClass
{
    public class MomoShopProductJobWorker : JobWorkerBase
    {
        public override Logger _logger { get => LogManager.GetCurrentClassLogger(); }
        private HtmlDocument htmlDoc = new HtmlDocument();
        public MomoShopProductJobWorker(JobInfo jobInfo)
        {
            this.jobInfo = jobInfo;
        }
        //private int driverId;
        public override JobInfo jobInfo { get; set; }

        protected override bool GotoNextPage(string url)
        {
            return false;
        }

        protected override bool Crawl()
        {
            try
            {
                //GetDriver();
                //OpenUrl();
                //responseData = GetData();       
                crawlDataDetailOptions.PutToDic("crawl_start", DateTime.UtcNow.ToString(RuleString.DateTimeFormat));
                responseData = SetHttpHeaderAndGetPageData();
                crawlDataDetailOptions.PutToDic("crawl_end", DateTime.UtcNow.ToString(RuleString.DateTimeFormat));
                return true;
            }
            catch (Exception ex)
            {
                LoggerHelper._.Error(ex);
                return false;
            }
        }


        private string SetHttpHeaderAndGetPageData()
        {
            //Uri uri = new Uri(jobInfo.Url);
            //HttpClientHandler handler = new HttpClientHandler() { CookieContainer = new CookieContainer() };
            //var httpClient = new HttpClient(handler);

            //var cookieCollection = CookiesHelper.GetCookies(Platform.MomoShop);
            //if (cookieCollection != null)
            //{
            //    handler.CookieContainer.Add(cookieCollection);
            //}
            //else
            //{
            //    httpClient.GetAsync("https://" + uri.Host).GetAwaiter().GetResult();
            //    var cookies = handler.CookieContainer.GetCookies(new Uri("https://" + uri.Host));
            //    CookiesHelper.SetCookies(Platform.MomoShop, cookies, 60);
            //}

            //HttpWebRequest request = HttpWebRequest.Create(jobInfo.Url) as HttpWebRequest;
            //request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.116 Safari/537.36";
            //request.Referer = "https://www.momoshop.com.tw";

            //using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            //{
            //    using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(response.CharacterSet)))
            //    {
            //        responseData = sr.ReadToEnd();
            //    }
            //}
            //return responseData;

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.116 Safari/537.36");
            httpClient.DefaultRequestHeaders.Add("referer", "https://www.momoshop.com.tw");
            var httpResponse = httpClient.GetAsync(jobInfo.Url).GetAwaiter().GetResult();
            return responseData = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
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
                crawlDataDetailOptions.PutToDic("parse_start", DateTime.UtcNow.ToString(RuleString.DateTimeFormat));
                htmlDoc.LoadHtml(responseData);

                //crawlDataDetailOptions.price = htmlDoc.DocumentNode.SelectSingleNode("//*[@class='priceTxtArea']//b").InnerText;
                //crawlDataDetailOptions.name = htmlDoc.DocumentNode.SelectSingleNode("//*[@id='goodsName']").InnerText;
                //crawlDataDetailOptions.category = string.Join(@">"
                //        , htmlDoc.DocumentNode.SelectNodes("//*[@class='pathArea']//a").Select(x => x.InnerText));

                crawlDataDetailOptions.price = string.Join("/"
                    , htmlDoc.DocumentNode.SelectNodes("//*[@class='prdnoteArea']//*[contains(@class, 'prdPrice')]//li")
                    .Where(x => Regex.IsMatch(HtmlEntity.DeEntitize(x.InnerText), @"\D+(價|價格)+(\d{1,3},)*\d+元"))
                    .Select(x => Regex.Match(HtmlEntity.DeEntitize(x.InnerText), @"\D+(價|價格)+(\d{1,3},)*\d+元").Value?
                        .Replace(System.Environment.NewLine, string.Empty).Trim()));

                crawlDataDetailOptions.name = HtmlEntity.DeEntitize(htmlDoc.DocumentNode.SelectSingleNode("//*[@class=\"prdnoteArea\"]//*[self::h1 or self::h2 or self::h3 or self::h4 or self::h5 or self::h6]").InnerText);
                crawlDataDetailOptions.category = string.Join("/", htmlDoc.DocumentNode.SelectNodes("//*[@id=\"bt_2_layout_NAV\"]/ul//li").Select(x => HtmlEntity.DeEntitize(x.InnerText)));
                crawlDataDetailOptions.PutToDic("_img", $"https:{htmlDoc.DocumentNode.SelectSingleNode("//*[@id='goodsimgB']//a[contains(@href, 'goodsimg')]")?.Attributes["href"].Value}");

                crawlDataDetailOptions.PutToDic("parse_end", DateTime.UtcNow.ToString(RuleString.DateTimeFormat));
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
                crawlDataDetailOptions.PutToDic("savedata_start", DateTime.UtcNow.ToString(RuleString.DateTimeFormat));
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

        //private void GetDriver()
        //{

        //    driverId = WebDriverPool.GetFreeDriver();

        //    WebDriverPool.DriverPool[driverId].Status = Common.Enums.ObjectStatus.Driver.NOTFREE;

        //}
        //private void OpenUrl()
        //{
        //    WebDriverPool.DriverPool[driverId].ChromeDriver.Navigate().GoToUrl(jobInfo.Url);
        //}



        //protected string GetData()
        //{
        //    string responseData = string.Empty;
        //    try
        //    {
        //        WebDriverPool.DriverPool[driverId].ChromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        //        responseData = WebDriverPool.DriverPool[driverId].ChromeDriver.FindElementByXPath("/html/body").GetAttribute("innerHTML");
        //        ScrollMove();

        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerHelper._.Error(ex);
        //    }
        //    finally
        //    {
        //        WebDriverPool.DriverPool[driverId].Status = Common.Enums.ObjectStatus.Driver.FREE;
        //    }
        //    return responseData;
        //}


        //private void ScrollMove()
        //{
        //    OpenQA.Selenium.IJavaScriptExecutor jse = WebDriverPool.DriverPool[driverId].ChromeDriver;
        //    int height = (int)Math.Ceiling(1000 * 0.1);
        //    jse.ExecuteScript("window.scrollBy(0," + height + ")");
        //}

        #endregion

    }
}
