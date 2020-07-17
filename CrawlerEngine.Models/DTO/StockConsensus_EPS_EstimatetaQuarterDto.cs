using System;

namespace CrawlerEngine.Model.DTO
{
    public class StockConsensus_EPS_EstimatetaQuarterDto
    {

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Code { get; set; }
        public float? Legal_estimate_EPS { get; set; }
        public string Legal_estimate_EPS_unit { get; set; }
        public float? Formula_estimate_EPS { get; set; }
        public string Formula_estimate_EPS_unit { get; set; }
        public float? Legal_estimated_yield { get; set; }
        public string Legal_estimated_yield_unit { get; set; }
        public float? Formula_estimated_yield { get; set; }
        public string Formula_estimated_yield_unit { get; set; }

    }
}
