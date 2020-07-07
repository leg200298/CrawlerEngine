using CrawlerEngine.Common.Helper;
using CrawlerEngine.Driver.WorkClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CrawlerEngine.Driver
{
    public class WebDriverPool
    {
        public static List<SeleniumDriver> DriverPool;
        private static object c = new object();
        private static int waitingTimes = 0;
        public static int GetFreeDriver()
        {
            if (DriverPool.Any(x => x.Status == Common.Enums.ObjectStatus.Driver.FREE))
            {
                lock (c)
                {
                    waitingTimes = 0;
                    return DriverPool.Where(x => x.Status == Common.Enums.ObjectStatus.Driver.FREE).First().id;
                }
            }
            else
            {
                waitingTimes++;
                if (waitingTimes > 3) { throw new Exception("沒有閒置資源 請稍後"); }
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

                LoggerHelper._.Error("InitDriverError", ex);
                throw ex;
            }
            return true;
        }
    }
}
