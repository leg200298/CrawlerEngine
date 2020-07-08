using OpenQA.Selenium.Chrome;

namespace CrawlerEngine.Driver.WorkClass
{
    public class SeleniumDriver
    {
        public Common.Enums.ObjectStatus.Driver Status = Common.Enums.ObjectStatus.Driver.FREE;
        public ChromeDriver ChromeDriver;
        public int id;

        public SeleniumDriver()
        {
            var chromeOptions = new ChromeOptions();
            //chromeOptions.AddArguments("headless");
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            ChromeDriver = new ChromeDriver(service, chromeOptions);
        }
        public SeleniumDriver Init(int id)
        {
            this.id = id;
            return this;
        }

    }
}
