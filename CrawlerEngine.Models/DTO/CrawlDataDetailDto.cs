using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrawlerEngine.Model.DTO
{
    public class CrawlDataDetailDto
    {
        [Column("seq")]
        public Guid seq { get; set; }
        [Column("detail_data")]
        public string detail_data { get; set; }
        [Column("job_status")]
        public string job_status { get; set; }
        [Column("end_time")]
        public DateTime? end_time { get; set; }
    }
}
