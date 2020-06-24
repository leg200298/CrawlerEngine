using CrawlerEngine.Driver.Interface;
using CrawlerEngine.Driver.WorkClass;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CrawlerEngine.Driver
{
    public class WebDriverPool
    {
        public static List<SeleniumDriver> DriverPool;
        public static SeleniumDriver GetFreeDriver()
        {
            throw new Exception();
        }
        public static bool InitDriver(int driverCount)
        {
            try
            {
                DriverPool.Clear();
                for (int i = 0; i < driverCount; ++i)
                {

                    DriverPool.Add(new SeleniumDriver().Init());
                }
            }
            catch (Exception ex) {

                throw ex;
                return false;
            }
            return true;
        }
    }
}
