using System;

namespace CrawlerEngine.Models
{
    public class JobInfo : Options
    {
        public Guid Seq { get; set; }

        /// <summary>
        /// 決定工作屬於的類型  ("平台"-"網頁類型")
        /// </summary>
        public string JobType
        {
            get { return GetJobType(); }
        }


        /// <summary>
        /// 目標網址
        /// </summary>
        public string Url
        {
            get { return GetUrl(); }
        }

        #region  private area
        private string GetUrl()
        {
            return GetString("_url");
        }
        private string GetJobType()
        {
            return GetString("_jobType");
        }
        #endregion
    }
}
