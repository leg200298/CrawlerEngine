using System;
using System.Collections.Generic;
using System.Text;

namespace CrawlerEngine.Model.DTO
{
    public class CrawlDataJobListDto
    {
        public Guid Seq { get; set; }
        public string JobInfo { get; set; }
        public DateTime RegisterTime { get; set; }
        public string JobStatus { get; set; }
        public DateTime? StartTime { get; set; }
    }
}
