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
            chromeOptions.AddArguments("headless");
            chromeOptions.AddArguments("no-sandbox");
            //chromeOptions.AddArguments("User-Agent=Googlebot");
            //chromeOptions.AddArguments("Referer=https://www.google.com");
            chromeOptions.AddArguments("Referer=https://ecshweb.pchome.com.tw/search/v3.3/?q=%E5%95%86%E5%93%81");
            chromeOptions.AddArguments("User-Agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.116 Safari/537.36");

            chromeOptions.AddArguments("disable-dev-shm-usage");
            chromeOptions.AddArguments("blink-settings=imagesEnabled=false");
            chromeOptions.AddArguments("disable-gpu");
            //chromeOptions.BinaryLocation = "/usr/bin/google-chrome-stable";
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
