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
            set { SetJobType(value); }
        }

        /// <summary>
        /// 目標網址
        /// </summary>
        public string Url
        {
            get { return GetUrl(); }
            set { SetUrl(value); }
        }

        #region  private area
        private string GetUrl()
        {
            return GetString("_url");
        }


        private void SetUrl(string value)
        {
            PutToDic("_url", value);
        }

        private string GetJobType()
        {
            return GetString("_jobType");
        }

        private void SetJobType(string value)
        {
            PutToDic("_jobType", value);
        }

        #endregion
    }
}
