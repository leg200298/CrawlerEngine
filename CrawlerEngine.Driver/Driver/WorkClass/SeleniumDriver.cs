using OpenQA.Selenium.Chrome;
using static CrawlerEngine.Common.NamingString.ObjectStatus;

namespace CrawlerEngine.Driver.WorkClass
{
    public class SeleniumDriver : ChromeDriver
    {
        public DriverStatus Status = DriverStatus.FREE;

        public SeleniumDriver Init()
        {
            return this;
        }

    }
}
