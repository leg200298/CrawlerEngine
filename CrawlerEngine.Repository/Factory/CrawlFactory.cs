using CrawlerEngine.Repository.Common.Helper;
using CrawlerEngine.Repository.Crawl;

namespace CrawlerEngine.Repository.Factory
{
    public class CrawlFactory
    {
        private static CrawlDataDetailRepository _crawlDataDetailRepository => new CrawlDataDetailRepository(new AzureDbConnectionHelper());
        private static CrawlDataJobListRepository _crawlDataJobListRepository => new CrawlDataJobListRepository(new AzureDbConnectionHelper());
        private static StockListRepository _stockListRepository => new StockListRepository(new AzureDbConnectionHelper());
        private static StockJobListRepository _stockJobListRepository => new StockJobListRepository(new AzureDbConnectionHelper());
        private static StockPriceDailyRepository _stockPriceDailyRepository => new StockPriceDailyRepository(new AzureDbConnectionHelper());
        private static StockEPSMonthlyRepository _stockEPSMonthlyRepository => new StockEPSMonthlyRepository(new AzureDbConnectionHelper());
        private static StockConsensus_EPS_EstimatetaQuarterRepository _stockConsensus_EPS_EstimatetaQuarterRepository => new StockConsensus_EPS_EstimatetaQuarterRepository(new AzureDbConnectionHelper());


        public static CrawlDataDetailRepository CrawlDataDetailRepository = _crawlDataDetailRepository;
        public static CrawlDataJobListRepository CrawlDataJobListRepository = _crawlDataJobListRepository;
        public static StockListRepository StockListRepository = _stockListRepository;
        public static StockJobListRepository StockJobListRepository = _stockJobListRepository;
        public static StockPriceDailyRepository StockPriceDailyRepository = _stockPriceDailyRepository;
        public static StockEPSMonthlyRepository StockEPSMonthlyRepository = _stockEPSMonthlyRepository;
        public static StockConsensus_EPS_EstimatetaQuarterRepository StockConsensus_EPS_EstimatetaQuarterRepository = _stockConsensus_EPS_EstimatetaQuarterRepository;


    }
}
