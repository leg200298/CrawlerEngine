using System;
using System.Collections.Generic;
using System.Text;

namespace CrawlerEngine.Crawler.Interface
{
    public interface ICrawler
    {

        DateTime StartTime { get; set; }

        string DoCrawlerFlow();
    }
}
