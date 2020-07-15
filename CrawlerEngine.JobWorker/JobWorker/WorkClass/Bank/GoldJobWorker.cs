using CrawlerEngine.Common.Helper;
using CrawlerEngine.Crawler.Interface;
using CrawlerEngine.Crawler.WorkClass;
using CrawlerEngine.Model.DTO;
using CrawlerEngine.Models;
using HtmlAgilityPack;
using System;
using System.Threading;

namespace CrawlerEngine.JobWorker.WorkClass
{

    /// <summary>
    /// 館分類頁
    /// </summary>
    class GoldJobWorker : JobWorkerBase
    {
        private HtmlDocument htmlDoc = new HtmlDocument();
        BankDto bankDto = new BankDto();
        public GoldJobWorker(JobInfo jobInfo)
        {
            this.jobInfo = jobInfo;
            crawler = new HttpCrawler(jobInfo);
        }
        public override JobInfo jobInfo { get; set; }
        public override ICrawler crawler { get; set; }

        protected override bool GotoNextPage(string url)
        {
            return false;
        }

        protected override bool Crawl()
        {
            var success = false;
            try
            {
                responseData = crawler.DoCrawlerFlow();
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

            var sellNode = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"ie11andabove\"]/div/div[2]/table[1]/tbody/tr[1]/td[3]");
            var buyNode = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"ie11andabove\"]/div/div[2]/table[1]/tbody/tr[2]/td[3]");
            bankDto.Date = DateTime.UtcNow;
            bankDto.Currency = "GOLD";
            bankDto.RateBuy = Convert.ToDouble(buyNode.InnerText.Replace(",", ""));
            bankDto.RateSell = Convert.ToDouble(sellNode.InnerText.Replace(",", ""));
            return true;
        }

        protected override bool SaveData()
        {

            Repository.Factory.CrawlFactory.BankInfoRepository.InsertOne(bankDto);

            return true;

        }

        protected override (bool, string) HasNextPage()
        {

            return (false, "");
        }

        protected override void SleepForAWhile(decimal sleepTime)
        {
            Thread.Sleep((int)(sleepTime * 1000));
        }


    }

}
