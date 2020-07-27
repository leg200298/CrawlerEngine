using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrawlerEngine.Model.DTO
{
    public class CrawlDataDetailDto
    {
        [Column("seq")]
        public Guid Seq { get; set; }
        [Column("detail_data")]
        public string DetailData { get; set; }
        [Column("job_status")]
        public string JobStatus { get; set; }
        [Column("end_time")]
        public DateTime? EndTime { get; set; }
    }
}
