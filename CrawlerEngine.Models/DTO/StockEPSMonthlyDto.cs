using System;

namespace CrawlerEngine.Model.DTO
{
    public class StockEPSMonthlyDto
    {

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Code { get; set; }
        public string Year { get; set; }
        public float? EPS_estimate_All_AVG_byMonth_latestonly { get; set; }
        public string EPS_estimate_All_AVG_byMonth_latestonly_unit { get; set; }

    }
}
