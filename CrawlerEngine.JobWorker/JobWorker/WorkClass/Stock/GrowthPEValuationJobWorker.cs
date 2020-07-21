using CrawlerEngine.Common.Helper;
using CrawlerEngine.Crawler.Interface;
using CrawlerEngine.Crawler.WorkClass;
using CrawlerEngine.Model.DTO;
using CrawlerEngine.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CrawlerEngine.JobWorker.WorkClass
{
    /// <summary>
    /// 商品細節頁
    /// </summary>
    public class GrowthPEValuationJobWorker : JobWorkerBase
    {
        Rootobject t = new Rootobject();
        List<StockEPSMonthlyDto> ls = new List<StockEPSMonthlyDto>();
        public GrowthPEValuationJobWorker(JobInfo jobInfo)
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
            ls.Add(new StockEPSMonthlyDto()
            {
                Code = t.data.stock_code,
                Date = DateTime.UtcNow,
                Year = "2019",
                EPS_estimate_All_AVG_byMonth_latestonly = t.data.refdata.uae10021_cp.Data
            ,
                EPS_estimate_All_AVG_byMonth_latestonly_unit = t.data.refdata.uae10021_cp.UnitRef
            });
            ls.Add(new StockEPSMonthlyDto()
            {
                Code = t.data.stock_code,
                Date = DateTime.UtcNow,
                Year = "2020",
                EPS_estimate_All_AVG_byMonth_latestonly = t.data.refdata.uae10022_cp.Data
,
                EPS_estimate_All_AVG_byMonth_latestonly_unit = t.data.refdata.uae10022_cp.UnitRef
            });
            ls.Add(new StockEPSMonthlyDto()
            {
                Code = t.data.stock_code,
                Date = DateTime.UtcNow,
                Year = "2021",
                EPS_estimate_All_AVG_byMonth_latestonly = t.data.refdata.uae10023_cp.Data
,
                EPS_estimate_All_AVG_byMonth_latestonly_unit = t.data.refdata.uae10023_cp.UnitRef
            });
            ls.Add(new StockEPSMonthlyDto()
            {
                Code = t.data.stock_code,
                Date = DateTime.UtcNow,
                Year = "2022",
                EPS_estimate_All_AVG_byMonth_latestonly = t.data.refdata.uae10024_cp.Data
,
                EPS_estimate_All_AVG_byMonth_latestonly_unit = t.data.refdata.uae10024_cp.UnitRef
            });


            return true;

        }

        protected override bool SaveData()
        {

            foreach (var d in ls) { Repository.Factory.CrawlFactory.StockEPSMonthlyRepository.InsertOne(d); }
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
                try
                {
                    t = JsonConvert.DeserializeObject<Rootobject>(responseData);

                    if (t.status.ToUpper() != "OK") throw new Exception("Api Error");
                    if (t.data == null) throw new Exception("No Data");
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
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
            public float? Data { get; set; }
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
            public float? Data { get; set; }
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
            public float? Data { get; set; }
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
            public float? Data { get; set; }
        }


    }



}
