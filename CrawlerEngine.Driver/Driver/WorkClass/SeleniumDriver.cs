using OpenQA.Selenium.Chrome;

namespace CrawlerEngine.Driver.WorkClass
{
    public class SeleniumDriver : ChromeDriver
    {
        public string Status = "FREE";

        public SeleniumDriver Init()
        {
            return this;
        }

    }
}
