using CrawlerEngine.Common.Extansion;
using CrawlerEngine.Common.Helper;
using CrawlerEngine.Crawler.Interface;
using CrawlerEngine.Driver;
using CrawlerEngine.Models;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using static CrawlerEngine.Common.Enums.ElectronicBusiness;

namespace CrawlerEngine.JobWorker.WorkClass
{
    public class MomoShopDgrpCategoryJobWorker : JobWorkerBase
    {
        public override JobInfo jobInfo { get; set; }        

        private List<JobInfo> jobInfos = new List<JobInfo>();
        private HtmlDocument htmlDoc = new HtmlDocument();
        private ResponseJObject responseJObject = null;
        private int driverId;
        public MomoShopDgrpCategoryJobWorker(JobInfo jobInfo)
        {
            this.jobInfo = jobInfo;
        }

        protected override bool GotoNextPage(string url)
        {
            jobInfos = new List<JobInfo>();
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
                responseData = SetHttpHeaderAndGetPageData();
                responseJObject = JsonConvert.DeserializeObject<ResponseJObject>(responseData);
                if (!responseJObject.rtnData.rtnGoodsData.success)
                {
                    GetDriver();
                    OpenUrl();
                    responseData = GetData();
                }
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
            Uri uri = new Uri(jobInfo.Url);
            HttpClientHandler handler = new HttpClientHandler()
            {
                CookieContainer = new CookieContainer()
            };
            var httpClient = new HttpClient(handler);
            httpClient.GetAsync("https://" + uri.Host).GetAwaiter().GetResult();

            httpClient.DefaultRequestHeaders.Add("Accept", "application/json, text/javascript, */*; q=0.01");
            httpClient.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.116 Safari/537.36");
            httpClient.DefaultRequestHeaders.Add("referer", "https://www.momoshop.com.tw");

            var postData = new StringContent(Convert.ToString(jobInfo.GetFromDic("_postData"))
                , Encoding.UTF8, "application/x-www-form-urlencoded");
            string url = Convert.ToString(jobInfo.GetFromDic("_apiUrl")) + "&t="
                + ((Int64)new TimeSpan(DateTime.UtcNow.Ticks - new DateTime(1970, 1, 1).Ticks)
                               .TotalMilliseconds).ToString();

            var httpResponse = httpClient.PostAsync(url, postData).GetAwaiter().GetResult();
            return httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
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
                var nodes = htmlDoc.DocumentNode.SelectNodes(@"//*[@id='prdlistArea' or contains(@class, 'prdListArea')]//a[contains(@href, 'GoodsDetail')]");
                if (nodes != null)
                {
                    foreach (var data in nodes)
                    {
                        string href = HtmlEntity.DeEntitize(data.Attributes["href"].Value);
                        jobInfos.Add(new JobInfo()
                        {
                            Seq = Guid.NewGuid(),
                            JobType = Platform.MomoShopProduct.GetDescription(),
                            Url = href.StartsWith("https://www.momoshop.com.tw") ? href : "https://www.momoshop.com.tw" + href
                        });
                    }
                    return true;
                }

                if (responseJObject != null && responseJObject.rtnData.rtnGoodsData.success)
                {
                    foreach (var item in responseJObject.rtnData.rtnGoodsData.rtnGoodsData.goodsInfoList)
                    {
                        string goodsCode = item.goodsCode;
                        string url = $"https://www.momoshop.com.tw/goods/GoodsDetail.jsp?i_code={goodsCode}";
                        string goodsName = item.goodsName;
                        string imgUrl = item.imgUrl;
                        string goodsPrice = item.goodsPrice;
                        string SALE_PRICE = item.SALE_PRICE;

                        jobInfos.Add(new JobInfo()
                        {
                            Seq = Guid.NewGuid(),
                            JobType = Platform.MomoShopProduct.GetDescription(),
                            Url = url
                        });
                    }
                    return true;
                }

                return false;
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
                foreach (var job in jobInfos)
                {
                    Repository.Factory.CrawlFactory.CrawlDataJobListRepository.InsertOne(job, Platform.MomoShopProduct.GetDescription());
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
            try
            {
                var page = htmlDoc.DocumentNode.SelectSingleNode("//*[(@class='pageArea topEnPageArea')]//a[contains(@name,'nextPage')]");
                if (page != null && page.Attributes["page"] != null)
                {
                    int.TryParse(Regex.Match(jobInfo.Url, @"&pageNum=\d+").Value.Split('=')
                         .Where(x => Regex.IsMatch(x, @"\d+")).FirstOrDefault(), out int pageIndex);
                    pageIndex = pageIndex == 0 ? pageIndex + 1 : pageIndex;

                    int.TryParse(page.Attributes["page"].Value, out int pageLast);

                    if (pageLast > pageIndex)
                    {
                        string nextUrl = Regex.Replace(jobInfo.Url, @"&pageNum=\d+", "") + $"&pageNum={pageIndex}";
                        return (true, nextUrl);
                    }
                    return (false, "");
                }
                else if (responseJObject != null && responseJObject.rtnData.rtnGoodsData.success)
                {

                    int maxPage = int.Parse(responseJObject.rtnData.rtnGoodsData.maxPage);
                    int curPage = int.Parse(responseJObject.rtnData.rtnGoodsData.curPage);

                    if (maxPage > curPage)
                    {
                        Int64.TryParse(Regex.Match(Convert.ToString(jobInfo.Url), @"d_code=\d+").Value.Split('=')
                            .Where(x => Regex.IsMatch(x, @"\d+")).FirstOrDefault(), out Int64 cateCode);

                        string postData = "data=" + Uri.EscapeDataString(
                            $"{{\"flag\":2035,\"data\":{{\"params\":{{\"cateCode\":\"{cateCode}\",\"cateLevel\":\"3\",\"curPage\":\"{curPage + 1}\"}}}}}}");

                        jobInfo.PutToDic("_postData", postData);
                        return (true, jobInfo.Url);
                    }
                    return (false, "");
                }
                else
                {
                    return (false, "");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper._.Error(ex);
                return (false, "");
            }


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

    public class ExtraValue
    {
        public string urlParameter { get; set; }
        public string cateType { get; set; }
        public string isWebPage { get; set; }
        public string categoryCode { get; set; }
    }

    public class Action
    {
        public int? actionType { get; set; }
        public string actionValue { get; set; }
        public bool? useDefault { get; set; }
        public ExtraValue extraValue { get; set; }

    }

    public class Top123
    {
        public string orderqty { get; set; }
        public string contentType { get; set; }
        public string imgUrl { get; set; }
        public List<string> imgUrlArray { get; set; }
        public string imgTagUrl { get; set; }
        public string goodsName { get; set; }
        public string goodsSubName { get; set; }
        public string @operator { get; set; }
        public string TvYn { get; set; }
        public string ArdYN { get; set; }
        public string ArsDiscount { get; set; }
        public string isSetGoods { get; set; }
        public string delyType { get; set; }
        public string delyTemp { get; set; }
        public string goodsPrice { get; set; }
        public string SALE_PRICE { get; set; }
        public string goodsStock { get; set; }
        public string canTipStock { get; set; }
        public string norestAllotMonth { get; set; }
        public string shopWay { get; set; }
        public bool? isTracked { get; set; }
        public string categoryCode { get; set; }
        public string categoryName { get; set; }
        public string goodsCode { get; set; }
        public string vodUrl { get; set; }
        public string promotUrl { get; set; }
        public bool? isAdultLimit { get; set; }
        public string edmBackgroundUrl { get; set; }
        public Action action { get; set; }
        public string goodsFeatureUrl { get; set; }

    }

    public class ExtraValue2
    {
        public string cateLevel { get; set; }
        public string cateName { get; set; }

    }

    public class Action2
    {
        public int? actionType { get; set; }
        public string actionValue { get; set; }
        public bool? useDefault { get; set; }
        public ExtraValue2 extraValue { get; set; }

    }

    public class CategoryCrumb
    {
        public string categoryCode { get; set; }
        public string categoryName { get; set; }
        public Action2 action { get; set; }

    }

    public class Icon
    {
        public string iconBgColor { get; set; }
        public string iconContentColor { get; set; }
        public string iconContent { get; set; }
        public string iconContentType { get; set; }

    }

    public class ExtraValue3
    {
        public string urlParameter { get; set; }
        public string cateType { get; set; }
        public string isWebPage { get; set; }
        public string categoryCode { get; set; }

    }

    public class Action3
    {
        public int? actionType { get; set; }
        public string actionValue { get; set; }
        public bool? useDefault { get; set; }
        public ExtraValue3 extraValue { get; set; }

    }

    public class GoodsInfoList
    {
        public string orderqty { get; set; }
        public string contentType { get; set; }
        public string imgUrl { get; set; }
        public List<string> imgUrlArray { get; set; }
        public string imgTagUrl { get; set; }
        public bool? isTVGoods { get; set; }
        public string goodsIconType { get; set; }
        public bool? isRegister { get; set; }
        public bool? useCounpon { get; set; }
        public bool? isDiscount { get; set; }
        public bool? haveGift { get; set; }
        public string cycleYn { get; set; }
        public string goodsName { get; set; }
        public string goodsSubName { get; set; }
        public string @operator { get; set; }
        public string TvYn { get; set; }
        public string ArdYN { get; set; }
        public string ArsDiscount { get; set; }
        public string isSetGoods { get; set; }
        public string delyType { get; set; }
        public string delyTemp { get; set; }
        public string goodsPrice { get; set; }
        public string SALE_PRICE { get; set; }
        public string goodsStock { get; set; }
        public string canTipStock { get; set; }
        public string norestAllotMonth { get; set; }
        public string shopWay { get; set; }
        public bool? isTracked { get; set; }
        public List<Icon> icon { get; set; }
        public string categoryCode { get; set; }
        public string categoryName { get; set; }
        public string goodsCode { get; set; }
        public string promotUrl { get; set; }
        public bool? isAdultLimit { get; set; }
        public string edmBackgroundUrl { get; set; }
        public Action3 action { get; set; }
        public string goodsFeatureUrl { get; set; }
        public string vodUrl { get; set; }

    }

    public class BrandName
    {
        public string brandTitle { get; set; }
        public List<string> brandName { get; set; }
        public List<int?> brandCount { get; set; }
        public List<string> brandNameStr { get; set; }

    }

    public class IndexInfoList
    {
        public string indexName { get; set; }
        public string indexNameNo { get; set; }
        public List<string> groupId { get; set; }
        public List<string> indexContent { get; set; }
        public List<string> indexContentStr { get; set; }
        public List<int?> indexContentCnt { get; set; }

    }

    public class RtnGoodsData2
    {
        public List<Top123> top123 { get; set; }
        public List<CategoryCrumb> categoryCrumbs { get; set; }
        public List<object> categoryList { get; set; }
        public List<GoodsInfoList> goodsInfoList { get; set; }
        public List<BrandName> brandName { get; set; }
        public List<IndexInfoList> indexInfoList { get; set; }

    }

    public class RtnGoodsData
    {
        public bool success { get; set; }
        public string resultCode { get; set; }
        public string resultMessage { get; set; }
        public string resultException { get; set; }
        public string maxPage { get; set; }
        public string minPage { get; set; }
        public string curPage { get; set; }
        public string pageSize { get; set; }
        public string totalCnt { get; set; }
        public string curPageGoodsCnt { get; set; }
        public string cateLevel { get; set; }
        public string shareTitle { get; set; }
        public bool? isAdultLimit { get; set; }
        public string categoryType { get; set; }
        public bool? isBrandCategory { get; set; }
        public RtnGoodsData2 rtnGoodsData { get; set; }
        public string endYn { get; set; }
        public string showSieveCondition { get; set; }
        public bool? isV2 { get; set; }
        public string MOMOMSGID { get; set; }

    }

    public class RtnData
    {
        public RtnGoodsData rtnGoodsData { get; set; }
    }

    public class ResponseJObject
    {
        public RtnData rtnData { get; set; }
        public string rtnMsg { get; set; }
        public int rtnCode { get; set; }

    }

}
