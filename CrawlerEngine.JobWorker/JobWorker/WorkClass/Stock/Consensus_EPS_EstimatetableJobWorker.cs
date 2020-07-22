using CrawlerEngine.Common.Helper;
using CrawlerEngine.Model.DTO;
using CrawlerEngine.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;

namespace CrawlerEngine.JobWorker.WorkClass
{
    /// <summary>
    /// 商品細節頁
    /// </summary>
    public class Consensus_EPS_EstimatetableJobWorker : JobWorkerBase
    {
        JObject t = new JObject();
        StockConsensus_EPS_EstimatetaQuarterDto scee = new StockConsensus_EPS_EstimatetaQuarterDto();
        public Consensus_EPS_EstimatetableJobWorker(JobInfo jobInfo)
        {
            this.jobInfo = jobInfo;
        }
        public override JobInfo jobInfo { get; set; }

        protected override bool Crawl()
        {
            var success = false;
            try
            {
                var httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromSeconds(5);
                var httpResponse = httpClient.GetAsync(jobInfo.Url).Result;
                responseData = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
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
            scee.Date = DateTime.UtcNow;
            scee.Code = jobInfo.Url.Split('/').LastOrDefault();
            scee.Legal_estimate_EPS = t["data"]["display"]["uae10021_cp"]["Data"].ToString() == "[]" ? null : (float?)t["data"]["display"]["uae10021_cp"]["Data"];
            scee.Formula_estimate_EPS = t["data"]["display"]["ua60001_cp"]["Data"].ToString() == "[]" ? null : (float?)t["data"]["display"]["ua60001_cp"]["Data"];
            scee.Legal_estimated_yield = t["data"]["display"]["uae10041_cp"]["Data"].ToString() == "[]" ? null : (float?)t["data"]["display"]["uae10041_cp"]["Data"];
            scee.Formula_estimated_yield = t["data"]["display"]["ua50052_cp"]["Data"].ToString() == "[]" ? null : (float?)t["data"]["display"]["ua50052_cp"]["Data"];
            scee.Legal_estimate_EPS_unit = (string)t["data"]["display"]["uae10021_cp"]["UnitRef"];
            scee.Formula_estimate_EPS_unit = (string)t["data"]["display"]["ua60001_cp"]["UnitRef"];
            scee.Legal_estimated_yield_unit = (string)t["data"]["display"]["uae10041_cp"]["UnitRef"];
            scee.Formula_estimated_yield_unit = (string)t["data"]["display"]["ua50052_cp"]["UnitRef"];
            return true;

        }

        protected override bool SaveData()
        {

            Repository.Factory.CrawlFactory.StockConsensus_EPS_EstimatetaQuarterRepository.InsertOne(scee);
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

                    try
                    {
                        t = JObject.Parse(responseData);
                    }
                    catch { throw new Exception($"Parse Error {responseData}"); }

                    if (t["status"].ToString().ToUpper() != "OK") throw new Exception("Api Error");
                    if (t["data"] == null) throw new Exception("No Data");
                    if (t["data"].ToString().ToLower() == "stock code not exist") throw new Exception("Code Not Exist");
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
            public object[] search_column { get; set; }
            public object[] column_title { get; set; }
            public bool is_multiple { get; set; }
            public object[] data { get; set; }
            public Display display { get; set; }
            public string type { get; set; }
        }

        public class Display
        {
            /// <summary>
            /// 法人估EPS(月平均)
            /// </summary>
            public Uae10021_Cp uae10021_cp { get; set; }
            /// <summary>
            /// 採用近四季EPS加總，當作全年度的EPS預估值參考。
            /// </summary>
            public Ua60001_Cp ua60001_cp { get; set; }
            /// <summary>
            /// 將法人預估EPS乘以上一次的股息配發率，計算出未來一年的預估現金股息；將此預估股息除以股價之後，可以得出未來一年預估殖利率，作為投資的參考。
            /// </summary>
            public Uae10041_Cp uae10041_cp { get; set; }
            /// <summary>
            /// 將近四季EPS加總之後，乘以上一次的股息配發率，計算出未來一年的預估現金股息；將此預估股息除以股價之後，可以得出未來一年預估殖利率，作為投資的參考。
            /// </summary>
            public Ua50052_Cp ua50052_cp { get; set; }
        }

        public class Uae10021_Cp
        {
            public int Order { get; set; }
            public string ChineseAccount { get; set; }
            public string UnitRef { get; set; }
            public string Explanation { get; set; }
            public string Style { get; set; }
            public float? Data { get; set; }
        }

        public class Ua60001_Cp
        {
            public int Order { get; set; }
            public string ChineseAccount { get; set; }
            public string UnitRef { get; set; }
            public string Explanation { get; set; }
            public string Style { get; set; }
            public float? Data { get; set; }
        }

        public class Uae10041_Cp
        {
            public int Order { get; set; }
            public string ChineseAccount { get; set; }
            public string UnitRef { get; set; }
            public string Explanation { get; set; }
            public string Style { get; set; }
            public float? Data { get; set; }
        }

        public class Ua50052_Cp
        {
            public int Order { get; set; }
            public string ChineseAccount { get; set; }
            public string UnitRef { get; set; }
            public string Explanation { get; set; }
            public string Style { get; set; }
            public float? Data { get; set; }
        }

    }
}
