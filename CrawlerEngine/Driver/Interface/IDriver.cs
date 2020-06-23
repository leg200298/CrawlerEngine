using System;
using System.Collections.Generic;
using System.Text;

namespace CrawlerEngine.Driver.Interface
{
    interface IDriver
    {
        String Status { get; set; }

        void Init();
    }
}
