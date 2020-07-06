using CrawlerEngine.Driver.WorkClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using static CrawlerEngine.Common.Enums.ObjectStatus;

namespace CrawlerEngine.Driver
{
    public class WebDriverPool
    {
        public static List<SeleniumDriver> DriverPool;
        private static object c = new object();

        public static int GetFreeDriver()
        {
            if (DriverPool.Any(x => x.Status == DriverStatus.FREE))
            {
                lock (c)
                {
                    return DriverPool.Where(x => x.Status == DriverStatus.FREE).First().id;
                    //q.Status = DriverStatus.NOTFREE;
                    //return q;
                }
            }
            else
            {
                Thread.Sleep(500);
                return GetFreeDriver();
            }
        }
        public static bool InitDriver(int driverCount)
        {
            try
            {
                if (DriverPool == null) { DriverPool = new List<SeleniumDriver>(); }
                DriverPool.Clear();
                for (int i = 0; i < driverCount; ++i)
                {

                    DriverPool.Add(new SeleniumDriver().Init(i));
                }
            }
            catch (Exception ex)
            {

                throw ex;
                return false;
            }
            return true;
        }
    }
}
