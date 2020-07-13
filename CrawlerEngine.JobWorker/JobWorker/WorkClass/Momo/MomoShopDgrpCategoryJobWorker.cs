using CrawlerEngine.Common.Extansion;
using CrawlerEngine.Common.Helper;
using CrawlerEngine.Crawler.Interface;
using CrawlerEngine.Crawler.WorkClass;
using CrawlerEngine.Models;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using static CrawlerEngine.Common.Enums.ElectronicBusiness;

namespace CrawlerEngine.JobWorker.WorkClass
{
    public class MomoShopDgrpCategoryJobWorker : JobWorkerBase
    {
        public override JobInfo jobInfo { get; set; }
        public override ICrawler crawler { get; set; }

        private List<JobInfo> jobInfos = new List<JobInfo>();
        private HtmlDocument htmlDoc = new HtmlDocument();
        private JObject jObject = new JObject();

        public MomoShopDgrpCategoryJobWorker(JobInfo jobInfo)
        {
            jobInfo.PutToHeaderDic("Accept", "application/json, text/javascript, */*; q=0.01");
            jobInfo.PutToHeaderDic("X-Requested-With", "XMLHttpRequest");
            jobInfo.PutToHeaderDic("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.116 Safari/537.36");
            jobInfo.PutToHeaderDic("referer", "https://www.momoshop.com.tw");

            this.jobInfo = jobInfo;
            crawler = new MomoHttpCrawler(jobInfo);
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
                responseData = crawler.DoCrawlerFlow();
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
                //htmlDoc.LoadHtml(responseData);
                //var nodes = htmlDoc.DocumentNode.SelectNodes("//*[@class='prdListArea']//a[contains(@href, 'goods.momo')]");
                //if (nodes is null) { return false; }

                jobInfos = new List<JobInfo>();
                jObject = JObject.Parse(responseData);

                foreach (var item in jObject["rtnData"]["rtnGoodsData"]["rtnGoodsData"]["goodsInfoList"])
                {
                    string goodsCode = item.Value<string>("goodsCode");
                    string url = $"https://www.momoshop.com.tw/goods/GoodsDetail.jsp?i_code={goodsCode}";
                    string goodsName = item.Value<string>("goodsName");
                    string imgUrl = item.Value<string>("imgUrl");
                    string goodsPrice = item.Value<string>("goodsPrice");
                    string SALE_PRICE = item.Value<string>("SALE_PRICE");

                    jobInfos.Add(new JobInfo()
                    {
                        Seq = Guid.NewGuid(),
                        JobType = Platform.MomoShopProduct.GetDescription(),                       
                        Url = url
                    });
                }
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
                foreach (var job in jobInfos)
                {
                    Repository.Factory.CrawlFactory.CrawlDataJobListRepository.InsertOne(job);
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

                int maxPage = jObject["rtnData"]["rtnGoodsData"].Value<int>("maxPage");
                int curPage = jObject["rtnData"]["rtnGoodsData"].Value<int>("curPage");

                if (maxPage > curPage)
                {
                    Int64.TryParse(Regex.Match(Convert.ToString(jobInfo.GetFromDic("_webSiteUrl")), @"(d_code=\d+|m_code=\d+)").Value.Split('=')
                        .Where(x => Regex.IsMatch(x, @"\d+")).FirstOrDefault(), out Int64 cateCode);

                    string postData = "data=" + Uri.EscapeDataString(
                        $"{{\"flag\":2035,\"data\":{{\"params\":{{\"cateCode\":\"{cateCode}\",\"cateLevel\":\"3\",\"curPage\":\"{curPage + 1}\"}}}}}}");

                    jobInfo.PutToDic("_postData", postData);
                    return (true, jobInfo.Url);
                }
                return (false, "");


                //var pages = htmlDoc.DocumentNode.SelectNodes("//*[@class='pageArea']//dd//a");
                //if (pages is null) { return (false, ""); }

                //int lastPageIndex = pages.Select(x => new { r = int.TryParse(x.InnerText, out int i), lastPageIndex = i })
                //    .Select(i => i.lastPageIndex)
                //    .OrderByDescending(o => o)
                //    .FirstOrDefault();

                //int.TryParse(Regex.Match(jobInfo.Url, @"&page=\d+").Value.Split('=')
                //         .Where(x => Regex.IsMatch(x, @"\d+")).FirstOrDefault(), out int pageIndex);

                //pageIndex = pageIndex == 0 ? pageIndex + 1 : pageIndex;

                //if (lastPageIndex > pageIndex)
                //{
                //    string next = Regex.Replace(jobInfo.Url, @"&page=\d+", "") + $"&page={pageIndex + 1}";
                //    return (true, next);
                //}
                //return (false, "");
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
    }
}
