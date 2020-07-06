using System;
using System.Collections.Generic;
using System.Text;

namespace CrawlerEngine.Crawler.Interface
{
    public interface ICrawler
    {

        DateTime StartTime { get; set; }
        /// <summary>
        /// 執行爬蟲流程
        /// </summary>
        /// <returns></returns>
        string DoCrawlerFlow();
    }
}
