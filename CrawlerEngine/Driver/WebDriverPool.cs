using CrawlerEngine.Driver.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CrawlerEngine.Driver
{
    class WebDriverPool
    {
        static List<IDriver> DriverPool;
        public IDriver GetFreeDriver()
        {
            throw new Exception();
        }
    }
}
