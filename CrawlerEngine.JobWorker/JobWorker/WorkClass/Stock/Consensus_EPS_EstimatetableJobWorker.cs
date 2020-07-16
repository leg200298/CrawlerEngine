using CrawlerEngine.Common.Helper;
using CrawlerEngine.Crawler.Interface;
using CrawlerEngine.Crawler.WorkClass;
using CrawlerEngine.Model.DTO;
using CrawlerEngine.Models;
using Newtonsoft.Json;
using System;

namespace CrawlerEngine.JobWorker.WorkClass
{
    /// <summary>
    /// 商品細節頁
    /// </summary>
    public class Consensus_EPS_EstimatetableJobWorker : JobWorkerBase
    {
        public Consensus_EPS_EstimatetableJobWorker(JobInfo jobInfo)
        {
            this.jobInfo = jobInfo;
            crawler = new HttpCrawler(jobInfo);
        }
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
            var t = JsonConvert.DeserializeObject<Rootobject>(responseData);
            var EPS_estimate_All_2019_AVG_byMonth_latestonly = t.data.refdata.uae10021_cp.Data + t.data.refdata.uae10021_cp.UnitRef;
            var EPS_estimate_All_2020_AVG_byMonth_latestonly = t.data.refdata.uae10022_cp.Data + t.data.refdata.uae10022_cp.UnitRef;
            var EPS_estimate_All_2021_AVG_byMonth_latestonly = t.data.refdata.uae10023_cp.Data + t.data.refdata.uae10023_cp.UnitRef;
            var EPS_estimate_All_2022_AVG_byMonth_latestonly = t.data.refdata.uae10024_cp.Data + t.data.refdata.uae10024_cp.UnitRef;
            //var htmlDoc = new HtmlDocument();
            //htmlDoc.LoadHtml(responseData);

            //crawlDataDetailOptions.price = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"PriceTotal\"]").InnerText;
            //crawlDataDetailOptions.name = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"NickContainer\"]").InnerText;
            //crawlDataDetailOptions.category = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"CONTENT\"]/div[1]/div[1]/div[2]").InnerText;
            return true;

        }

        protected override bool SaveData()
        {
            CrawlDataDetailDto crawlDataDetailDto = new CrawlDataDetailDto()
            {
                Seq = jobInfo.Seq,
                JobStatus = "end",
                EndTime = DateTime.UtcNow,
                DetailData = crawlDataDetailOptions.GetJsonString()
            };

            Repository.Factory.CrawlFactory.CrawlDataDetailRepository.InsertDataDetail(crawlDataDetailDto);
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


        public class Rootobject
        {
            public string status { get; set; }
            public Data data { get; set; }
            public int c { get; set; }
        }

        public class Data
        {
            public Data1 data { get; set; }
            public Refdata refdata { get; set; }
            public string stock_name { get; set; }
            public string stock_code { get; set; }
            public string type { get; set; }
        }

        public class Data1
        {
        }

        public class Refdata
        {
            /// <summary>
            /// 2019_EPS預估(月平均值)-最新值
            /// </summary>
            public Uae10021_Cp uae10021_cp { get; set; }
            /// <summary>
            /// 2020_EPS預估(月平均值)-最新值
            /// </summary>
            public Uae10022_Cp uae10022_cp { get; set; }
            /// <summary>
            /// 2021_EPS預估(月平均值)-最新值
            /// </summary>
            public Uae10023_Cp uae10023_cp { get; set; }
            /// <summary>
            /// 2022_EPS預估(月平均值)-最新值
            /// </summary>
            public Uae10024_Cp uae10024_cp { get; set; }
        }

        public class Uae10021_Cp
        {
            public int Order { get; set; }
            public int Sorting { get; set; }
            public string EnglishAccount { get; set; }
            public string ChineseAccount { get; set; }
            public string UnitRef { get; set; }
            public string Explanation { get; set; }
            public string Style { get; set; }
            public float Data { get; set; }
        }

        public class Uae10022_Cp
        {
            public int Order { get; set; }
            public int Sorting { get; set; }
            public string EnglishAccount { get; set; }
            public string ChineseAccount { get; set; }
            public string UnitRef { get; set; }
            public string Explanation { get; set; }
            public string Style { get; set; }
            public float Data { get; set; }
        }

        public class Uae10023_Cp
        {
            public int Order { get; set; }
            public int Sorting { get; set; }
            public string EnglishAccount { get; set; }
            public string ChineseAccount { get; set; }
            public string UnitRef { get; set; }
            public string Explanation { get; set; }
            public string Style { get; set; }
            public float Data { get; set; }
        }

        public class Uae10024_Cp
        {
            public int Order { get; set; }
            public int Sorting { get; set; }
            public string EnglishAccount { get; set; }
            public string ChineseAccount { get; set; }
            public string UnitRef { get; set; }
            public string Explanation { get; set; }
            public string Style { get; set; }
            public object[] Data { get; set; }
        }

    }
}
