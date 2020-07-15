using CrawlerEngine.Common.Helper;
using CrawlerEngine.Crawler.Interface;
using CrawlerEngine.Crawler.WorkClass;
using CrawlerEngine.Model.DTO;
using CrawlerEngine.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Threading;

namespace CrawlerEngine.JobWorker.WorkClass
{

    /// <summary>
    /// 館分類頁
    /// </summary>
    class BankExchangeJobWorker : JobWorkerBase
    {
        private List<BankDto> bankInfos = new List<BankDto>();
        private HtmlDocument htmlDoc = new HtmlDocument();
        public BankExchangeJobWorker(JobInfo jobInfo)
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

            bankInfos.Add(NewBankDto("USD"));
            bankInfos.Add(NewBankDto("HKD"));
            bankInfos.Add(NewBankDto("GBP"));
            bankInfos.Add(NewBankDto("AUD"));
            bankInfos.Add(NewBankDto("CAD"));
            bankInfos.Add(NewBankDto("SGD"));
            bankInfos.Add(NewBankDto("CHF"));
            bankInfos.Add(NewBankDto("JPY"));
            bankInfos.Add(NewBankDto("ZAR"));
            bankInfos.Add(NewBankDto("SEK"));
            bankInfos.Add(NewBankDto("NZD"));
            bankInfos.Add(NewBankDto("THB"));
            bankInfos.Add(NewBankDto("EUR"));
            bankInfos.Add(NewBankDto("CNY"));
            return true;
        }

        private BankDto NewBankDto(string currency)
        {
            var buyNode = htmlDoc.DocumentNode.SelectSingleNode(BankXPath(currency, "BUY"));
            var sellNode = htmlDoc.DocumentNode.SelectSingleNode(BankXPath(currency, "SELL"));
            BankDto bankDto = new BankDto();
            bankDto.Date = DateTime.UtcNow;
            bankDto.Currency = currency;
            bankDto.RateBuy = Convert.ToDouble(buyNode.InnerText.Replace(",", ""));
            bankDto.RateSell = Convert.ToDouble(sellNode.InnerText.Replace(",", ""));
            return bankDto;
        }


        protected override bool SaveData()
        {

            foreach (var d in bankInfos)
            {
                Repository.Factory.CrawlFactory.BankInfoRepository.InsertOne(d);
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
        private string BankXPath(string currency, string type)
        {

            string xpath = string.Empty;
            var currencyUP = currency.ToUpper();
            var typeUP = type.ToUpper();
            if (currencyUP == "USD")
            {
                if (typeUP == "BUY") xpath = "//*[@id=\"ie11andabove\"]/div/table/tbody/tr[1]/td[4]";
                if (typeUP == "SELL") xpath = "//*[@id=\"ie11andabove\"]/div/table/tbody/tr[1]/td[5]";
            }
            if (currencyUP == "HKD")
            {
                if (typeUP == "BUY") xpath = "//*[@id=\"ie11andabove\"]/div/table/tbody/tr[2]/td[4]";
                if (typeUP == "SELL") xpath = "//*[@id=\"ie11andabove\"]/div/table/tbody/tr[2]/td[5]";
            }
            if (currencyUP == "GBP")
            {
                if (typeUP == "BUY") xpath = "//*[@id=\"ie11andabove\"]/div/table/tbody/tr[3]/td[4]";
                if (typeUP == "SELL") xpath = "//*[@id=\"ie11andabove\"]/div/table/tbody/tr[3]/td[5]";
            }
            if (currencyUP == "AUD")
            {
                if (typeUP == "BUY") xpath = "//*[@id=\"ie11andabove\"]/div/table/tbody/tr[4]/td[4]";
                if (typeUP == "SELL") xpath = "//*[@id=\"ie11andabove\"]/div/table/tbody/tr[4]/td[5]";
            }
            if (currencyUP == "CAD")
            {
                if (typeUP == "BUY") xpath = "//*[@id=\"ie11andabove\"]/div/table/tbody/tr[5]/td[4]";
                if (typeUP == "SELL") xpath = "//*[@id=\"ie11andabove\"]/div/table/tbody/tr[5]/td[5]";
            }
            if (currencyUP == "SGD")
            {
                if (typeUP == "BUY") xpath = "//*[@id=\"ie11andabove\"]/div/table/tbody/tr[6]/td[4]";
                if (typeUP == "SELL") xpath = "//*[@id=\"ie11andabove\"]/div/table/tbody/tr[6]/td[5]";
            }
            if (currencyUP == "CHF")
            {
                if (typeUP == "BUY") xpath = "//*[@id=\"ie11andabove\"]/div/table/tbody/tr[7]/td[4]";
                if (typeUP == "SELL") xpath = "//*[@id=\"ie11andabove\"]/div/table/tbody/tr[7]/td[5]";
            }
            if (currencyUP == "JPY")
            {
                if (typeUP == "BUY") xpath = "//*[@id=\"ie11andabove\"]/div/table/tbody/tr[8]/td[4]";
                if (typeUP == "SELL") xpath = "//*[@id=\"ie11andabove\"]/div/table/tbody/tr[8]/td[5]";
            }
            if (currencyUP == "ZAR")
            {
                if (typeUP == "BUY") xpath = "//*[@id=\"ie11andabove\"]/div/table/tbody/tr[9]/td[4]";
                if (typeUP == "SELL") xpath = "//*[@id=\"ie11andabove\"]/div/table/tbody/tr[9]/td[5]";
            }
            if (currencyUP == "SEK")
            {
                if (typeUP == "BUY") xpath = "//*[@id=\"ie11andabove\"]/div/table/tbody/tr[10]/td[4]";
                if (typeUP == "SELL") xpath = "//*[@id=\"ie11andabove\"]/div/table/tbody/tr[10]/td[5]";
            }
            if (currencyUP == "NZD")
            {
                if (typeUP == "BUY") xpath = "//*[@id=\"ie11andabove\"]/div/table/tbody/tr[11]/td[4]";
                if (typeUP == "SELL") xpath = "//*[@id=\"ie11andabove\"]/div/table/tbody/tr[11]/td[5]";
            }
            if (currencyUP == "THB")
            {
                if (typeUP == "BUY") xpath = "//*[@id=\"ie11andabove\"]/div/table/tbody/tr[12]/td[4]";
                if (typeUP == "SELL") xpath = "//*[@id=\"ie11andabove\"]/div/table/tbody/tr[12]/td[5]";
            }
            if (currencyUP == "EUR")
            {
                if (typeUP == "BUY") xpath = "//*[@id=\"ie11andabove\"]/div/table/tbody/tr[15]/td[4]";
                if (typeUP == "SELL") xpath = "//*[@id=\"ie11andabove\"]/div/table/tbody/tr[15]/td[5]";
            }
            if (currencyUP == "CNY")
            {
                if (typeUP == "BUY") xpath = "//*[@id=\"ie11andabove\"]/div/table/tbody/tr[19]/td[4]";
                if (typeUP == "SELL") xpath = "//*[@id=\"ie11andabove\"]/div/table/tbody/tr[19]/td[5]";
            }

            return xpath;
        }

    }


}
