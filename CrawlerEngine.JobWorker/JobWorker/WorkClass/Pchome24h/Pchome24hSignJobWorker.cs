using CrawlerEngine.Common.Extansion;
using CrawlerEngine.Common.Helper;
using CrawlerEngine.Crawler.WorkClass;
using CrawlerEngine.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Threading;
using static CrawlerEngine.Common.Enums.ElectronicBusiness;

namespace CrawlerEngine.JobWorker.WorkClass
{

    /// <summary>
    /// 館分類頁
    /// </summary>
    class Pchome24hSignJobWorker : JobWorkerBase
    {
        private List<JobInfo> jobInfos = new List<JobInfo>();
        private HtmlDocument htmlDoc = new HtmlDocument();
        public Pchome24hSignJobWorker(JobInfo jobInfo)
        {
            this.jobInfo = jobInfo;
        }
        public override JobInfo jobInfo { get; set; }

        protected override bool GotoNextPage(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }
            else
            {
                if (url.StartsWith("//24h.pchome.com.tw"))
                {
                    jobInfo.Url = $"https:{url}";
                }

                return true;
            }
        }

        protected override bool Crawl()
        {
            var success = false;
            try
            {
                responseData = new WebCrawler(jobInfo).DoCrawlerFlow();
                success = true;
            }
            catch (Exception ex)
            {
                LoggerHelper._.Error(ex);
            }
            return success;
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

            htmlDoc.LoadHtml(responseData);
            #region region
            HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"ToothContainer\"]/div/ul/li/a");
            if (nodes is null) { return false; }
            foreach (var data in nodes)
            {
                var url = data.Attributes["href"].Value;
                if (url.StartsWith("//24h.pchome.com.tw"))
                {
                    jobInfos.Add(new JobInfo() { JobType = Platform.Pchome24hRegion.GetDescription(), Url = $"https:{url}" });
                }
            }
            #endregion store
            nodes = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"BLK10\"]/dl/dd[2]/div/ul/li/a");
            if (nodes is null) { return false; }
            foreach (var data in nodes)
            {
                var url = data.Attributes["href"].Value;
                if (url.StartsWith("//24h.pchome.com.tw"))
                {
                    jobInfos.Add(new JobInfo() { JobType = Platform.Pchome24hStore.GetDescription(), Url = $"https:{url}" });
                }
            }
            nodes = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"BLK09\"]/dl/dd[2]/div/ul/li/a");
            if (nodes is null) { return false; }
            foreach (var data in nodes)
            {
                var url = data.Attributes["href"].Value;
                if (url.StartsWith("//24h.pchome.com.tw"))
                {
                    jobInfos.Add(new JobInfo() { JobType = Platform.Pchome24hStore.GetDescription(), Url = $"https:{url}" });
                }
            }
            return true;
        }

        protected override bool SaveData()
        {

            foreach (var d in jobInfos)
            {
                Repository.Factory.CrawlFactory.CrawlDataJobListRepository.InsertOne(d, Platform.Pchome24hStore.GetDescription());
            }
            return true;

        }

        protected override (bool, string) HasNextPage()
        {

            //htmlDoc.LoadHtml(responseData);

            //var nodes = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"PaginationContainer\"]/ul/li/a");

            //foreach (var node in nodes)
            //{
            //    if (node.InnerText == "下一頁")
            //    {
            //        return (true, node.Attributes["href"].Value);
            //    }
            //}
            return (false, "");
        }

        protected override void SleepForAWhile(decimal sleepTime)
        {
            Thread.Sleep((int)(sleepTime * 1000));
        }


    }
}
