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
            //httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.105 Safari/537.36");
            //httpClient.DefaultRequestHeaders.Add("Referer", jobInfo.Url);

            //httpClient.DefaultRequestHeaders.Add("User-Agent", "Googlebot");

            //httpClient.DefaultRequestHeaders.Add("Referer", "https://www.google.com");

            httpClient.DefaultRequestHeaders.Add("if-modified-since", "Wed, 05 Aug 2020 07:19:40 GMT");
            httpClient.DefaultRequestHeaders.Add("cookie", "ECC=b8e15585a23f4bd600b19d46374059235d83cdde.1485782368; E=zRHrOYjNV72DcEKG9ysZXv02CZCJy%2FLy0pPR5Y5ZaIYAYfMKaXQT9aOd1J%2FKxHPE6rY9TAqtAyXBLcZXVkHJQihtCOoSxBHF; ECWEBSESS=fd2cc93926.b31bebc8cad0786c92aec522b560552eb3cb28b3.1509933443; venguid=bab70cd8-c8c6-45ed-a4e4-41ac4e98ed40.instance-group-venapi-fn0t20181216; puuid=K.20190428212238.72; uuid=0d710e9c-f407-4322-921a-7e4d056b1c3e; _gcl_au=1.1.1863783388.1594038055; _fbp=fb.2.1594038056459.2066246521; U=a6ab26ff126bde983e390a48d2548d5aab1f196a; PCHOMEUNIQID=041d156e5b4c7cb1fc71562154c6684b; _pahc_t=1594312694; CID=ae193f7bde466bb822d4810eb053a02e1d3355d1; X=8417702; MBR=tm731531%40hotmail.com; gsite=24h; PRI_tm731531%40hotmail.com=0; HistoryEC=%7B%22P%22%3A%5B%7B%22Id%22%3A%22DSAA31-A900A6PB5%22%2C%20%22M%22%3A1596981729%7D%2C%20%7B%22Id%22%3A%22DJAA2V-A900AM97O%22%2C%20%22M%22%3A1596882835%7D%5D%2C%20%22T%22%3A1%7D; _gid=GA1.3.770563525.1596981731; vensession=9259417d-3cd8-4c5b-b07d-d8038b6783f2.wg1-1n4020200809.se; _ga=GA1.1.1839673043.1529073435; _ga_9CE1X6J1FG=GS1.1.1596981729.13.0.1596981756.0");


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
                    catch {
                        throw new Exception($"Parse Error {responseData}");
                    }

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
