using CrawlerEngine.Common.Helper;
using CrawlerEngine.Model.DTO;
using CrawlerEngine.Models;
using HtmlAgilityPack;
using System;
using System.Net.Http;
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
        }
        public override JobInfo jobInfo { get; set; }

        protected override bool GotoNextPage(string url)
        {
            return false;
        }

        protected override bool Crawl()
        {
            var success = false;
            try
            {
                var httpClient = new HttpClient();
                foreach (var key in jobInfo.HeaderDic.Keys)
                {
                    httpClient.DefaultRequestHeaders.Add(key, jobInfo.HeaderDic[key]);
                }
                var httpResponse = httpClient.GetAsync(jobInfo.Url).GetAwaiter().GetResult();

                responseData = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
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
