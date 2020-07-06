using OpenQA.Selenium.Chrome;
using static CrawlerEngine.Common.NamingString.ObjectStatus;

namespace CrawlerEngine.Driver.WorkClass
{
    public class SeleniumDriver : ChromeDriver
    {
        public DriverStatus Status = DriverStatus.FREE;
        public int id;

        public SeleniumDriver Init(int id)

        {
            this.id = id;
            return this;
        }

    }
}
