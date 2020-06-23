using System;
using System.Collections.Generic;
using System.Text;

namespace CrawlerEngine.Driver.Interface
{
    public interface IDriver
    {
        String Status { get; set; }

        void Init();
    }
}
