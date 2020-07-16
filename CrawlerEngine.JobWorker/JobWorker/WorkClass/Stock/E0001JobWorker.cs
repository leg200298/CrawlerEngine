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
    public class E0001JobWorker : JobWorkerBase
    {
        public E0001JobWorker(JobInfo jobInfo)
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
            var 股價 = t.data.display.ua80010_cp.Data + t.data.display.ua80010_cp.UnitRef;
            var five年盈餘年複合成長率 = t.data.display.ua50018_cp.Data + t.data.display.ua50018_cp.UnitRef;
            var three年盈餘年複合成長率 = t.data.display.ua50019_cp.Data + t.data.display.ua50019_cp.UnitRef;
            var 近一季盈餘年增率 = t.data.display.ua60012_cp.Data + t.data.display.ua60012_cp.UnitRef;
            var 近四季EPS = t.data.display.ua60001_cp.Data + t.data.display.ua60001_cp.UnitRef;
            var 本益比 = t.data.display.ua80012_cp.Data + t.data.display.ua80012_cp.UnitRef;
            var PEG_5年複合成長率 = t.data.display.ua50078_cp.Data + t.data.display.ua50078_cp.UnitRef;
            var 填權息機率_五年 = t.data.display.ua80038_cp.Data + t.data.display.ua80038_cp.UnitRef;


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
            public float response_time { get; set; }
            public int c { get; set; }
        }

        public class Data
        {
            public Data1 data { get; set; }
            public Display display { get; set; }
            public string stock_name { get; set; }
            public string stock_code { get; set; }
            public string type { get; set; }
        }

        public class Data1
        {
        }

        public class Display
        {
            /// <summary>
            /// 股價
            /// </summary>
            public Ua80010_Cp ua80010_cp { get; set; }
            /// <summary>
            /// 5年盈餘年複合成長率
            /// </summary>
            public Ua50018_Cp ua50018_cp { get; set; }
            /// <summary>
            /// 3年盈餘年複合成長率
            /// </summary>
            public Ua50019_Cp ua50019_cp { get; set; }
            /// <summary>
            /// 近一季盈餘年增率
            /// </summary>
            public Ua60012_Cp ua60012_cp { get; set; }
            /// <summary>
            /// 近四季EPS
            /// </summary>
            public Ua60001_Cp ua60001_cp { get; set; }
            /// <summary>
            /// 本益比
            /// </summary>
            public Ua80012_Cp ua80012_cp { get; set; }
            /// <summary>
            /// PEG(5年複合成長率)
            /// </summary>
            public Ua50078_Cp ua50078_cp { get; set; }
            /// <summary>
            /// 填權息機率(五年)
            /// </summary>
            public Ua80038_Cp ua80038_cp { get; set; }
        }

        public class Ua80010_Cp
        {
            public int Order { get; set; }
            public string ChineseAccount { get; set; }
            public string UnitRef { get; set; }
            public string Explanation { get; set; }
            public string Style { get; set; }
            public float Data { get; set; }
        }

        public class Ua50018_Cp
        {
            public int Order { get; set; }
            public string ChineseAccount { get; set; }
            public string UnitRef { get; set; }
            public string Explanation { get; set; }
            public string Style { get; set; }
            public float Data { get; set; }
        }

        public class Ua50019_Cp
        {
            public int Order { get; set; }
            public string ChineseAccount { get; set; }
            public string UnitRef { get; set; }
            public string Explanation { get; set; }
            public string Style { get; set; }
            public float Data { get; set; }
        }

        public class Ua60012_Cp
        {
            public int Order { get; set; }
            public string ChineseAccount { get; set; }
            public string UnitRef { get; set; }
            public string Explanation { get; set; }
            public string Style { get; set; }
            public int Data { get; set; }
        }

        public class Ua60001_Cp
        {
            public int Order { get; set; }
            public string ChineseAccount { get; set; }
            public string UnitRef { get; set; }
            public string Explanation { get; set; }
            public string Style { get; set; }
            public float Data { get; set; }
        }

        public class Ua80012_Cp
        {
            public int Order { get; set; }
            public string ChineseAccount { get; set; }
            public string UnitRef { get; set; }
            public string Explanation { get; set; }
            public string Style { get; set; }
            public float Data { get; set; }
        }

        public class Ua50078_Cp
        {
            public int Order { get; set; }
            public string ChineseAccount { get; set; }
            public string UnitRef { get; set; }
            public string Explanation { get; set; }
            public string Style { get; set; }
            public float Data { get; set; }
        }

        public class Ua80038_Cp
        {
            public int Order { get; set; }
            public string ChineseAccount { get; set; }
            public string UnitRef { get; set; }
            public string Explanation { get; set; }
            public string Style { get; set; }
            public int Data { get; set; }
        }

    }
}
