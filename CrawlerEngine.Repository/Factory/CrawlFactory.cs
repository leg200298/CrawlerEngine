using CrawlerEngine.Repository.Common.Helper;
using CrawlerEngine.Repository.Crawl;

namespace CrawlerEngine.Repository.Factory
{
    public class CrawlFactory
    {
        //#if DEBUG
        //private static CrawlDataDetailRepository _crawlDataDetailRepository => new CrawlDataDetailRepository(new AzureDbConnectionHelper());
        //private static CrawlDataJobListRepository _crawlDataJobListRepository => new CrawlDataJobListRepository(new AzureDbConnectionHelper());

        private static BankInfoRepository _bankInfoRepository => new BankInfoRepository(new AzureDbConnectionHelper());

        //#else

        //        private static CrawlDataDetailRepository _crawlDataDetailRepository => new CrawlDataDetailRepository(new SensenDbConnectionHelper());
        //        private static CrawlDataJobListRepository _crawlDataJobListRepository => new CrawlDataJobListRepository(new SensenDbConnectionHelper());

        //#endif
        //public static CrawlDataDetailRepository CrawlDataDetailRepository = _crawlDataDetailRepository;
        //public static CrawlDataJobListRepository CrawlDataJobListRepository = _crawlDataJobListRepository;
        public static BankInfoRepository BankInfoRepository = _bankInfoRepository;

    }
}
