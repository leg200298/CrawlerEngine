using CrawlerEngine.Common.Helper;
using CrawlerEngine.Model.DTO;
using CrawlerEngine.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;

namespace CrawlerEngine.JobWorker.WorkClass
{
    /// <summary>
    /// 商品細節頁
    /// </summary>
    public class E0001JobWorker : JobWorkerBase
    {
        StockPriceDailyDto stockPriceDailyDto = new StockPriceDailyDto();
        JObject t = new JObject();
        public E0001JobWorker(JobInfo jobInfo)
        {
            jobInfo.PutToHeaderDic("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.116 Safari/537.36");

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



            stockPriceDailyDto.share_price =(float?) t["data"]["display"]["ua80010_cp"]["Data"];
            stockPriceDailyDto.share_price_unit = (string)t["data"]["display"]["ua80010_cp"]["UnitRef"];
            stockPriceDailyDto.five_year_surplus_compound_annual_growth_rate = (float?)t["data"]["display"]["ua50018_cp"]["Data"];
            stockPriceDailyDto.five_year_surplus_compound_annual_growth_rate_unit = (string)t["data"]["display"]["ua50018_cp"]["UnitRef"];
            stockPriceDailyDto.three_year_surplus_compound_annual_growth_rate = (float?)t["data"]["display"]["ua50019_cp"]["Data"];
            stockPriceDailyDto.three_year_surplus_compound_annual_growth_rate_unit = (string)t["data"]["display"]["ua50019_cp"]["UnitRef"];
            stockPriceDailyDto.annual_surplus_growth_rate_in_the_last_quarter = (float?)t["data"]["display"]["ua60012_cp"]["Data"];
            stockPriceDailyDto.annual_surplus_growth_rate_in_the_last_quarter_unit = (string)t["data"]["display"]["ua60012_cp"]["UnitRef"];
            stockPriceDailyDto.recent_Four_Seasons_EPS = (float?)t["data"]["display"]["ua60001_cp"]["Data"];
            stockPriceDailyDto.Recent_Four_Seasons_EPS_unit = (string)t["data"]["display"]["ua60001_cp"]["UnitRef"];
            stockPriceDailyDto.PE_ratio = (float?)t["data"]["display"]["ua80012_cp"]["Data"];
            stockPriceDailyDto.PE_ratio_unit = (string)t["data"]["display"]["ua80012_cp"]["UnitRef"];
            stockPriceDailyDto.PEG_5_year_compound_growth_rate = (float?)t["data"]["display"]["ua50078_cp"]["Data"];
            stockPriceDailyDto.PEG_5_year_compound_growth_rate_unit = (string)t["data"]["display"]["ua50078_cp"]["UnitRef"];
            stockPriceDailyDto.Probability_to_fill_interest_five_years=(float?) t["data"]["display"]["ua80038_cp"]["Data"];
            stockPriceDailyDto.Probability_to_fill_interest_five_years_unit = (string)t["data"]["display"]["ua80038_cp"]["UnitRef"];
            stockPriceDailyDto.Code = (string)t["data"]["stock_code"]  ;
            return true;

        }

        protected override bool SaveData()
        {

            Repository.Factory.CrawlFactory.StockPriceDailyRepository.InsertOne(stockPriceDailyDto);
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
            public float? Data { get; set; }
        }

        public class Ua50018_Cp
        {
            public int Order { get; set; }
            public string ChineseAccount { get; set; }
            public string UnitRef { get; set; }
            public string Explanation { get; set; }
            public string Style { get; set; }
            public float? Data { get; set; }
        }

        public class Ua50019_Cp
        {
            public int Order { get; set; }
            public string ChineseAccount { get; set; }
            public string UnitRef { get; set; }
            public string Explanation { get; set; }
            public string Style { get; set; }
            public float? Data { get; set; }
        }

        public class Ua60012_Cp
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

        public class Ua80012_Cp
        {
            public int Order { get; set; }
            public string ChineseAccount { get; set; }
            public string UnitRef { get; set; }
            public string Explanation { get; set; }
            public string Style { get; set; }
            public float? Data { get; set; }
        }

        public class Ua50078_Cp
        {
            public int Order { get; set; }
            public string ChineseAccount { get; set; }
            public string UnitRef { get; set; }
            public string Explanation { get; set; }
            public string Style { get; set; }
            public float? Data { get; set; }
        }

        public class Ua80038_Cp
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
