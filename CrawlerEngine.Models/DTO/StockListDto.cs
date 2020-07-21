using System;

namespace CrawlerEngine.Model.DTO
{
    public class StockListDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }
    }
}
