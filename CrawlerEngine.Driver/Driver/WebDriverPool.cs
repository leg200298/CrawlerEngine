using CrawlerEngine.Driver.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CrawlerEngine.Driver
{
    public class WebDriverPool
    {
        public static List<IDriver> DriverPool;
        public static IDriver GetFreeDriver()
        {
            throw new Exception();
        }
    }
}
