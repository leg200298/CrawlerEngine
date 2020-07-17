using CrawlerEngine.Common.Extansion;
using CrawlerEngine.Common.Helper;
using CrawlerEngine.Crawler.Interface;
using CrawlerEngine.Crawler.WorkClass;
using CrawlerEngine.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using static CrawlerEngine.Common.Enums.ElectronicBusiness;

namespace CrawlerEngine.JobWorker.WorkClass
{
    public class MomoShopSearchJobWorker : JobWorkerBase
    {
        public override JobInfo jobInfo { get; set; }
        public override ICrawler crawler { get; set; }

        private List<JobInfo> jobInfos = new List<JobInfo>();
        private HtmlDocument htmlDoc = new HtmlDocument();

        public MomoShopSearchJobWorker(JobInfo jobInfo)
        {
            this.jobInfo = jobInfo;
            crawler = new WebCrawler(jobInfo);
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
                htmlDoc.LoadHtml(responseData);
                var nodes = htmlDoc.DocumentNode.SelectNodes(@"//div[@class='listArea']//a[contains(@href, 'GoodsDetail')]");
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
                var pages = htmlDoc.DocumentNode.SelectNodes("//*[(@class='pageArea topPage')]//dl//a");
                if (pages != null)
                {
                    int lastPageIndex = pages.Select(x => new { r = int.TryParse(x.Attributes["pageidx"].Value, out int i), lastPageIndex = i })
                          .Select(i => i.lastPageIndex)
                          .OrderByDescending(o => o)
                          .FirstOrDefault();

                    int.TryParse(Regex.Match(jobInfo.Url, @"&curPage=\d+").Value.Split('=')
                        .Where(x => Regex.IsMatch(x, @"\d+")).FirstOrDefault(), out int pageIndex);
                    pageIndex = pageIndex == 0 ? pageIndex + 1 : pageIndex;

                    if (lastPageIndex > pageIndex)
                    {
                        string nextUrl = Regex.Replace(jobInfo.Url, @"&curPage=\d+", "") + $"&curPage={pageIndex + 1}";
                        return (true, nextUrl);
                    }                
                }
                return (false, "");
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
