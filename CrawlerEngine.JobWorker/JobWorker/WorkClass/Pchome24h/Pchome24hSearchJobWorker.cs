using CrawlerEngine.Common.Helper;
using CrawlerEngine.Model.DTO;
using CrawlerEngine.Models;
using CrawlerEngine.Repository.Factory;
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

        }
        public override JobInfo jobInfo { get; set; }

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
                httpClient = SetHttpHeader(httpClient);
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

        private static HttpClient SetHttpHeader(HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/71.0.3578.98 Safari/537.36");
            httpClient.DefaultRequestHeaders.Add("referer", "https://ecshweb.pchome.com.tw/search/v3.3/?q=%E5%95%86%E5%93%81");
            httpClient.DefaultRequestHeaders.Add("method", "GET");
            httpClient.DefaultRequestHeaders.Add("cookie", "ECC=d120d8544129d086967c000ebe2708c81f256b45.1595988170; _gcl_aw=GCL.1595988225.EAIaIQobChMIo7qoqazx6gIV0quWCh2p0w49EAAYASAAEgJXAvD_BwE; _gcl_au=1.1.2076238395.1595988225; venguid=3aaf4963-b8e0-4682-b255-a50f23ab5d4d.wg1-36wz20200729; _gid=GA1.3.252842066.1595988226; _gac_UA-115564493-1=1.1595988226.EAIaIQobChMIo7qoqazx6gIV0quWCh2p0w49EAAYASAAEgJXAvD_BwE; uuid=xxx-1d11dc0d-c1b8-427e-b207-6cad9c3917ab; puuid=K.20200729100347.1; _fbp=fb.2.1595988226545.277087068; U=1bc5f9f05f716c814447745780f7d1ba5a980ce2; ECWEBSESS=5bfc43a020.59f93fd5ba19b35bd2bee7be735eeca301ef7a52.1596010380; _ga_9CE1X6J1FG=GS1.1.1596010378.2.0.1596010378.0; vensession=cd661a1e-537e-4c42-831b-12d77ecc8f95.wg1-36wz20200729.se; _ga=GA1.3.931605215.1595988226");
            
            
            return httpClient;
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

        protected override bool SaveData(CrawlFactory crawlFactory)
        {

            foreach (var data in lCrawlDataDetailOptions)
            {
                crawlFactory.CrawlDataDetailRepository.InsertDataDetail(
                    new CrawlDataDetailDto()
                    {
                        seq = jobInfo.Seq,
                        job_status = "end",
                        end_time = DateTime.UtcNow,
                        detail_data = data.GetJsonString()
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
