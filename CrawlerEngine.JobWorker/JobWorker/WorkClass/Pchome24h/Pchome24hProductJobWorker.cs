using CrawlerEngine.Common.Helper;
using CrawlerEngine.Driver;
using CrawlerEngine.Model.DTO;
using CrawlerEngine.Models;
using CrawlerEngine.Repository.Factory;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Linq;
using System.Net.Http;

namespace CrawlerEngine.JobWorker.WorkClass
{
    /// <summary>
    /// 商品細節頁
    /// </summary>
    public class Pchome24hProductJobWorker : JobWorkerBase
    {
        public override Logger _logger { get => LogManager.GetCurrentClassLogger(); }
        public Pchome24hProductJobWorker(JobInfo jobInfo)
        {
            this.jobInfo = jobInfo;
        }
        JObject t = new JObject();
        public override JobInfo jobInfo { get; set; }
        private int driverId;
        protected override bool Crawl()
        {
            var success = false;
            try
            {

                Uri uri = new Uri(jobInfo.Url);
                var productId = uri.Segments.LastOrDefault();
                var li = productId.Split('-');
                var store = li[0];

                var httpClient = new HttpClient();
                httpClient = SetHttpHeader(httpClient);
                var targetUrl = $"https://ecapi.pchome.com.tw/ecshop/prodapi/v2/prod/{productId}&store={store}&fields=Seq,Id,Name,Nick,PreOrdDate,SpeOrdDate,Price,Discount,Pic,Weight,ISBN,Qty,Bonus,isBig,isSpec,isCombine,isDiy,isRecyclable,isCarrier,isMedical,isBigCart,isSnapUp,isDescAndIntroSync,isFoodContents,isHuge,isEnergySubsidy,isPrimeOnly,isPreOrder24h,isWarranty,isLegalStore,isFresh,isBidding,isSet,Volume,isArrival24h,isETicket,ShipType&_callback=jsonp_prod&1596165300";
                var httpResponse = httpClient.GetAsync(targetUrl).GetAwaiter().GetResult();

                responseData = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                responseData = responseData.Replace("try{jsonp_prod(", "");
                responseData = responseData.Replace(");}catch(e){if(window.console){console.log(e);}}", "");
                //GetDriver();
                //OpenUrl();
                //responseData = GetData();
                success = true;
            }
            catch (Exception ex)
            {
                LoggerHelper._.Error(ex);
            }
            return success;
        }

        private HttpClient SetHttpHeader(HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.105 Safari/537.36");
            httpClient.DefaultRequestHeaders.Add("Referer", jobInfo.Url);



            return httpClient;
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
            var item = t.Children().FirstOrDefault() as JProperty;
            JObject tw = t[item.Name] as JObject;
            crawlDataDetailOptions.price = tw["Price"]["P"];
            crawlDataDetailOptions.name = tw["Nick"];
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
                try
                {
                    try
                    {
                        t = JObject.Parse(responseData);
                    }
                    catch { throw new Exception($"Parse Error {responseData}"); }

                    //if (t["status"].ToString().ToUpper() != "OK") throw new Exception("Api Error");
                    //if (t["data"] == null) throw new Exception("No Data");
                    //if (t["data"].ToString().ToLower() == "stock code not exist") throw new Exception("Code Not Exist");
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
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
