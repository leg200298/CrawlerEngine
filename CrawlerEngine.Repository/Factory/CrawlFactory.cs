using CrawlerEngine.Repository.Common.Helper;
using CrawlerEngine.Repository.Crawl;

namespace CrawlerEngine.Repository.Factory
{
    public class CrawlFactory
    {
        private static CrawlDataDetailRepository _crawlDataDetailRepository => new CrawlDataDetailRepository(new SensenDbConnectionHelper());
        private static CrawlDataJobListRepository _crawlDataJobListRepository => new CrawlDataJobListRepository(new SensenDbConnectionHelper());
        public static CrawlDataDetailRepository CrawlDataDetailRepository = _crawlDataDetailRepository;
        public static CrawlDataJobListRepository CrawlDataJobListRepository = _crawlDataJobListRepository;

    }
}
