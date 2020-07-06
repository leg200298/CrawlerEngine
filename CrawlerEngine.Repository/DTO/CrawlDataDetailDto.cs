using System;
using System.Collections.Generic;
using System.Text;

namespace CrawlerEngine.Repository.DTO
{
    public class CrawlDataDetailDto
    {
        public Guid Seq { get; set; }
        public string DetailData { get; set; }
        public string JobStatus { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
