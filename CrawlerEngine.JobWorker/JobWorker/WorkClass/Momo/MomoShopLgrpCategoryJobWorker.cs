using CrawlerEngine.Crawler.Interface;
using CrawlerEngine.Crawler.WorkClass;
using CrawlerEngine.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CrawlerEngine.JobWorker.WorkClass
{
    public class MomoShopLgrpCategoryJobWorker : JobWorkerBase
    {
        public override JobInfo jobInfo { get; set; }
        public override ICrawler crawler { get; set; }

        private List<JobInfo> jobInfos = new List<JobInfo>();
        private HtmlDocument htmlDoc = new HtmlDocument();

        public MomoShopLgrpCategoryJobWorker(JobInfo jobInfo)
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
                var nodes = htmlDoc.DocumentNode.SelectNodes(@"//*[@id='bt_category_Content']//a");
                if (nodes != null)
                {
                    foreach (var data in nodes)
                    {
                        if (data.InnerText != "更多")
                        {
                            string href = HtmlEntity.DeEntitize(data.Attributes["href"].Value);
                            jobInfos.Add(new JobInfo()
                            {
                                Seq = Guid.NewGuid(),
                                JobType = "MOMOSHOP-DGRPCATEGORY",
                                Url = href.StartsWith("https://www.momoshop.com.tw") ? href : "https://www.momoshop.com.tw" + href
                            });
                        }                        
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
            return (false, "");
        }


        protected override void SleepForAWhile(decimal sleepTime)
        {
            Thread.Sleep((int)(sleepTime * 1000));
        }

        protected override void UpdateJobStatusStart()
        {
            Repository.Factory.CrawlFactory.CrawlDataJobListRepository.UpdateStatusStart(jobInfo);
        }

        protected override void UpdateJobStatusEnd()
        {
            Repository.Factory.CrawlFactory.CrawlDataJobListRepository.UpdateStatusEnd(jobInfo);
        }


    }
}
