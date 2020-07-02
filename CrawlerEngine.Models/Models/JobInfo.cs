using System;

namespace CrawlerEngine.Models
{
    public class JobInfo : Options
    {
        public Guid Seq { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string JobType
        {
            get { return GetJobType(); }
            set { }
        }
        public string GetUrl()
        {
            return GetString("_url");
        }
        private string GetJobType()
        {
            return GetString("_jobType");
        }
    }
}
