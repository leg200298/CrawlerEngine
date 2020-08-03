using CrawlerEngine.Repository.Common.Helper;
using CrawlerEngine.Repository.Interface;
using CrawlerEngine.Repository.MSSQL;
using CrawlerEngine.Repository.PostgresSQL;

namespace CrawlerEngine.Repository.Factory
{
    public class CrawlFactory
    {
        //#if DEBUG
        //private MSSQLCrawlDataDetailRepository _crawlDataDetailRepository => new MSSQLCrawlDataDetailRepository(new AzureDbConnectionHelper());
        //private ICrawlDataJobListRepository _crawlDataJobListRepository => new MSSQLCrawlDataJobListRepository(new AzureDbConnectionHelper());

        //#else

        //        private static CrawlDataDetailRepository _crawlDataDetailRepository => new CrawlDataDetailRepository(new SensenDbConnectionHelper());
        //        private static CrawlDataJobListRepository _crawlDataJobListRepository => new CrawlDataJobListRepository(new SensenDbConnectionHelper());

        //#endif

        public CrawlFactory(string DBname)
        {
            DBname = DBname.ToUpper();
            if (DBname == "MSSQL")
            {
                CrawlDataJobListRepository = new MSSQLCrawlDataJobListRepository(new AzureDbConnectionHelper());
                CrawlDataDetailRepository = new MSSQLCrawlDataDetailRepository(new AzureDbConnectionHelper());
            }
            if (DBname == "POSTGRESSQL")
            {
                CrawlDataJobListRepository = new PostgresSQLCrawlDataJobListRepository(new PostgresDbConnectionHelper());
                CrawlDataDetailRepository = new PostgresSQLCrawlDataDetailRepository(new PostgresDbConnectionHelper());
            }
        }
        public ICrawlDataDetailRepository CrawlDataDetailRepository { get; set; }
        public ICrawlDataJobListRepository CrawlDataJobListRepository { get; set; }

    }
}
