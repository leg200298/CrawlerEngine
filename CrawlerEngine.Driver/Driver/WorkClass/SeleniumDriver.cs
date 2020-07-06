using OpenQA.Selenium.Chrome;

namespace CrawlerEngine.Driver.WorkClass
{
    public class SeleniumDriver : ChromeDriver
    {
        public Common.Enums.ObjectStatus.Driver Status = Common.Enums.ObjectStatus.Driver.FREE;
        public int id;

        public SeleniumDriver Init(int id)

        {
            this.id = id;
            return this;
        }

    }
}
