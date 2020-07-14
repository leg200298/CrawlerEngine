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
        private ICrawler webCrawler = null;

        private List<JobInfo> jobInfos = new List<JobInfo>();
        private HtmlDocument htmlDoc = new HtmlDocument();
        private JObject jObject = null;

        public MomoShopDgrpCategoryJobWorker(JobInfo jobInfo)
        {
            jobInfo.PutToHeaderDic("Accept", "application/json, text/javascript, */*; q=0.01");
            jobInfo.PutToHeaderDic("X-Requested-With", "XMLHttpRequest");
            jobInfo.PutToHeaderDic("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.116 Safari/537.36");
            jobInfo.PutToHeaderDic("referer", "https://www.momoshop.com.tw");

            this.jobInfo = jobInfo;
            crawler = new MomoHttpCrawler(jobInfo);
            webCrawler = new WebCrawler(jobInfo);
        }

        protected override bool GotoNextPage(string url)
        {
            jobInfos = new List<JobInfo>();
            jObject = null;
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
                JObject Result = JObject.Parse(responseData);
                //if (string.IsNullOrEmpty(Regex.Replace(responseData, @"\\r\\n", "")))
                bool.TryParse(Convert.ToString(Result["rtnData"]["rtnGoodsData"]["success"]), out bool checkResult);
                if (!checkResult)
                {
                    responseData = webCrawler.DoCrawlerFlow();
                }
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
                }
                else
                {
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
                var page = htmlDoc.DocumentNode.SelectSingleNode("//*[(@class='pageArea topEnPageArea')]//a[contains(@name,'nextPage')]");
                if (page != null && page.Attributes["page"] != null)
                {
                    string next = Regex.Replace(jobInfo.Url, @"&pageNum=\d+", "") + $"&pageNum={page.Attributes["page"].Value}";
                    return (true, next);
                }
                else if (jObject != null)
                {
                    int maxPage = jObject["rtnData"]["rtnGoodsData"].Value<int>("maxPage");
                    int curPage = jObject["rtnData"]["rtnGoodsData"].Value<int>("curPage");

                    if (maxPage > curPage)
                    {
                        Int64.TryParse(Regex.Match(Convert.ToString(jobInfo.Url), @"(d_code=\d+|m_code=\d+)").Value.Split('=')
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
    }
}
