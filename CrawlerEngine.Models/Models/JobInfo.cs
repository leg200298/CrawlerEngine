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
            get => GetJobType();
            set => SetJobType(value);
        }

        /// <summary>
        /// 目標網址
        /// </summary>
        public string Url
        {
            get => GetUrl();
            set => SetUrl(value);
        }
        /// <summary>
        /// 指定瀏覽器睡眠時間
        /// </summary>
        public decimal? DriverSleepTime => GetSleepTime();
        /// <summary>
        /// 取得睡覺時間
        /// </summary>
        /// <returns></returns>
        private decimal? GetSleepTime()
        {
            var st = GetString("_sleepTime"); ;
            if (string.IsNullOrEmpty(st))
            {
                return null;
            }

            return Convert.ToDecimal(st);
        }

        /// <summary>
        /// 資料紀錄時間
        /// </summary>
        public string RegisterTime => GetRegisterTime();


        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string ErrorInfo
        {
            get => GetErrorInfo();
            set => SetErrorInfo(value);
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

        private string GetRegisterTime()
        {
            return GetString("_registerTime");
        }

        private string GetErrorInfo()
        {
            return GetString("_errorInfo");
        }

        private void SetErrorInfo(string value)
        {
            PutToDic("_errorInfo", value);
        }

        #endregion
    }
}
