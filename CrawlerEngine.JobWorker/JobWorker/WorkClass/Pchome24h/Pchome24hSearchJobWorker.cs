using CrawlerEngine.Common.Helper;
using CrawlerEngine.Crawler.Interface;
using CrawlerEngine.Crawler.WorkClass;
using CrawlerEngine.Model.DTO;
using CrawlerEngine.Models;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Web;

namespace CrawlerEngine.JobWorker.WorkClass
{

    /// <summary>
    /// 館分類頁
    /// </summary>
    class Pchome24hSearchJobWorker : JobWorkerBase
    {
        private List<JobInfo> jobInfos = new List<JobInfo>();
        private HtmlDocument htmlDoc = new HtmlDocument();
        public Pchome24hSearchJobWorker(JobInfo jobInfo)
        {


            this.jobInfo = jobInfo;

            crawler = new WebCrawler(jobInfo);
        }
        public override JobInfo jobInfo { get; set; }
        public override ICrawler crawler { get; set; }

        private ResponseObject tempResponseObject = new ResponseObject();

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
            var success = false;
            try
            {
                var httpClient = new HttpClient();
                SetHttpHeader(httpClient); var httpResponse = httpClient.GetAsync(jobInfo.Url).GetAwaiter().GetResult();

                responseData = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                success = true;
            }
            catch (Exception ex)
            {
                LoggerHelper._.Error(ex);
            }
            return success;
        }

        private static void SetHttpHeader(HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/71.0.3578.98 Safari/537.36");
            httpClient.DefaultRequestHeaders.Add("Referer", "https://ecshweb.pchome.com.tw/search/v3.3/");
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
                tempResponseObject = JsonConvert.DeserializeObject<ResponseObject>(responseData);

                lCrawlDataDetailOptions.Clear();
                foreach (var data in tempResponseObject.prods)
                {
                    lCrawlDataDetailOptions.Add(new Models.Models.JsonOptions() { name = data.name, price = data.price });
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected override bool SaveData()
        {

            foreach (var data in lCrawlDataDetailOptions)
            {
                Repository.Factory.CrawlFactory.CrawlDataDetailRepository.InsertDataDetail(
                    new CrawlDataDetailDto()
                    {
                        Seq = jobInfo.Seq,
                        JobStatus = "end",
                        EndTime = DateTime.UtcNow,
                        DetailData = data.GetJsonString()
                    }
                    );

            };

            return true;

        }

        protected override (bool, string) HasNextPage()
        {

            Uri myUri = new Uri(jobInfo.Url);
            string paramQ = HttpUtility.ParseQueryString(myUri.Query).Get("q");
            string paramPage = HttpUtility.ParseQueryString(myUri.Query).Get("page");
            if (tempResponseObject.totalPage < Convert.ToInt32(paramPage))
            {
                return (false, "");
            }
            else
            {

                var url = new Uri(myUri, $"?q={paramQ}&page={Convert.ToInt32(paramPage) + 1}").ToString();

                return (true, url);
            }
        }

        protected override void SleepForAWhile(decimal sleepTime)
        {
            Thread.Sleep((int)(sleepTime * 1000));
        }





    }
    public class ResponseObject
    {
        public int QTime { get; set; }
        public int totalRows { get; set; }
        public int totalPage { get; set; }
        public Range range { get; set; }
        public string cateName { get; set; }
        public string q { get; set; }
        public string subq { get; set; }
        public string[] token { get; set; }
        public int isMust { get; set; }
        public Prod[] prods { get; set; }
    }

    public class Range
    {
        public string min { get; set; }
        public string max { get; set; }
    }

    public class Prod
    {
        public string Id { get; set; }
        public string cateId { get; set; }
        public string picS { get; set; }
        public string picB { get; set; }
        public string name { get; set; }
        public string describe { get; set; }
        public int price { get; set; }
        public int originPrice { get; set; }
        public string author { get; set; }
        public string brand { get; set; }
        public string publishDate { get; set; }
        public string sellerId { get; set; }
        public int isPChome { get; set; }
        public int isNC17 { get; set; }
        public string[] couponActid { get; set; }
        public string BU { get; set; }
    }

}
