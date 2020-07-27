using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrawlerEngine.Model.DTO
{
    public class CrawlDataJobListDto
    {
        [Column("seq")]
        public Guid Seq { get; set; }
        [Column("job_type")]
        public string JobType { get; set; }
        [Column("job_info")]
        public string JobInfo { get; set; }
        [Column("register_time")]
        public DateTime RegisterTime { get; set; }
        [Column("job_status")]
        public string JobStatus { get; set; }
        [Column("start_time")]
        public DateTime? StartTime { get; set; }
        [Column("end_time")]
        public DateTime? EndTime { get; set; }
        [Column("error_info")]
        public string ErrorInfo { get; set; }

    }
}
