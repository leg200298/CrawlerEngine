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

namespace CrawlerEngine.JobWorker.WorkClass
{
    public class MomoShopDgrpCategoryJobWorker : JobWorkerBase
    {
        public override JobInfo jobInfo { get; set; }
        public override ICrawler crawler { get; set; }

        private List<JobInfo> jobInfos = new List<JobInfo>();
        private HtmlDocument htmlDoc = new HtmlDocument();

        public MomoShopDgrpCategoryJobWorker(JobInfo jobInfo)
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
            catch (Exception)
            {
                return false;
                throw;
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
                var nodes = htmlDoc.DocumentNode.SelectNodes(@"//*[@id='prdlistArea' or contains(@class, 'prdListArea')]//a[contains(@class, 'prdUrl')]");
                if (nodes != null)
                {
                    foreach (var data in nodes)
                    {
                        string href = HtmlEntity.DeEntitize(data.Attributes["href"].Value);
                        jobInfos.Add(new JobInfo()
                        {
                            Seq = Guid.NewGuid(),
                            JobType = "MOMOSHOP-PRODUCT",
                            Url = href.StartsWith("https://www.momoshop.com.tw") ? href : "https://www.momoshop.com.tw" + href
                        });
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
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
            catch (Exception)
            {
                throw;
            }
        }

        protected override (bool, string) HasNextPage()
        {            
            var nodes = htmlDoc.DocumentNode.SelectNodes("//*[contains(@class, 'pageArea')]//a");
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    if (node.InnerText == "下一頁" && node.Attributes["pageidx"] != null)
                    {
                        return (true,
                           Regex.Replace(jobInfo.Url, @"&pageNum=\d+", "") + "&pageNum=" + node.Attributes["pageidx"].Value);
                    }
                }
            }
            return (false, "");
        }



        protected override void SleepForAWhile(decimal sleepTime)
        {
            Thread.Sleep((int)(sleepTime * 1000));
        }

        protected override void UpdateJobStatusEnd()
        {
            Repository.Factory.CrawlFactory.CrawlDataJobListRepository.UpdateStatusEnd(jobInfo);
        }

        protected override void UpdateJobStatusStart()
        {
            Repository.Factory.CrawlFactory.CrawlDataJobListRepository.UpdateStatusStart(jobInfo);
        }


    }
}
