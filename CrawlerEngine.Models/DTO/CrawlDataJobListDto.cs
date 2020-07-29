using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrawlerEngine.Model.DTO
{
    public class CrawlDataJobListDto
    {
        [Column("seq")]
        public Guid seq { get; set; }
        [Column("job_type")]
        public string job_type { get; set; }
        [Column("job_info")]
        public string job_info { get; set; }
        [Column("register_time")]
        public DateTime register_time { get; set; }
        [Column("job_status")]
        public string job_status { get; set; }
        [Column("start_time")]
        public DateTime? start_time { get; set; }
        [Column("end_time")]
        public DateTime? end_time { get; set; }
        [Column("error_info")]
        public string error_info { get; set; }

    }
}
