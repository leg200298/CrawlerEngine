using CrawlerEngine.Common.Helper;
using CrawlerEngine.Crawler.Interface;
using CrawlerEngine.Crawler.WorkClass;
using CrawlerEngine.Model.DTO;
using CrawlerEngine.Models;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CrawlerEngine.JobWorker.WorkClass
{
    /// <summary>
    /// 商品細節頁
    /// </summary>
    public class GoogleMapJobWorker : JobWorkerBase
    {
        public GoogleMapJobWorker(JobInfo jobInfo)
        {
            this.jobInfo = jobInfo;
            crawler = new HttpCrawler(jobInfo);
        }
        PublicToiletInfoDto publicToiletInfoDto = new PublicToiletInfoDto();
        public override JobInfo jobInfo { get; set; }
        public override ICrawler crawler { get; set; }

        protected override bool Crawl()
        {
            var success = false;
            try
            {
                // new HttpCrawler(new JobInfo() { Url = "https://24h.pchome.com.tw/store/DSAACI" }).DoCrawlerFlow();
                responseData = crawler.DoCrawlerFlow();
                success = true;
            }
            catch (Exception ex)
            {
                LoggerHelper._.Error(ex);
            }
            return success;
        }
        protected override bool GotoNextPage(string url)
        {
            return false;
        }

        protected override (bool, string) HasNextPage()
        {

            return (false, "");
        }

        protected override bool Parse()
        {

            var cc = responseData.Replace(")]}'", "");

           var t= JsonConvert.DeserializeObject<List<Object>>(cc);
            var c = t[37] as string[];
            var ta =JsonConvert.DeserializeObject<List<Object>>(t[37].ToString());
            var tq= JsonConvert.DeserializeObject<Object>(ta[2].ToString());
            var qqq = JsonConvert.DeserializeObject<List<Object>>(tq.ToString());
            var qqqq = JsonConvert.DeserializeObject<List<Object>>(qqq[0].ToString());
            var qqqqq = JsonConvert.DeserializeObject<List<Object>>(qqqq[3].ToString());


            //address
            publicToiletInfoDto.toilet_address = qqqq[4].ToString();
            publicToiletInfoDto.toilet_latitude = Convert.ToDouble(qqqqq[2].ToString());
            publicToiletInfoDto.toilet_longitude = Convert.ToDouble(qqqqq[3].ToString());
            //var htmlDoc = new HtmlDocument();
            //htmlDoc.LoadHtml(responseData);

            //crawlDataDetailOptions.price = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"PriceTotal\"]").InnerText;
            //crawlDataDetailOptions.name = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"NickContainer\"]").InnerText;
            //crawlDataDetailOptions.category = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"CONTENT\"]/div[1]/div[1]/div[2]").InnerText;
            return true;

        }

        protected override bool SaveData()
        {

            Repository.Factory.CrawlFactory.PublicToiletInfoRepository.InsertPublicToiletInfo(publicToiletInfoDto);
            return true;

        }

        protected override void SleepForAWhile(decimal sleepTime)
        {

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
    }



    public class Rootobject
    {
        public  List<Object> Property1 { get; set; }
    }

}
