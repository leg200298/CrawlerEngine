using System;
using System.Collections.Generic;
using System.Text;

namespace CrawerEngine.Crawler.Interface
{
    interface ICrawler
    {

        DateTime StartTime { get; set; }

        string DoCrawlerFlow();
    }
}
