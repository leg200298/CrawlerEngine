using System;

namespace CrawlerEngine.Model.DTO
{
    public class BankDto
    {
        public Int64 Seq { get; set; }
        public DateTime Date { get; set; }
        public string Currency { get; set; }
        public double RateSell { get; set; }
        public double RateBuy { get; set; }

    }
}
