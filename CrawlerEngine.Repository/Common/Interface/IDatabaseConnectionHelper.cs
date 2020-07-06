using System.Data;

namespace CrawlerEngine.Repository.Common.Interface
{
    public interface IDatabaseConnectionHelper
    {
        IDbConnection Create();
    }
}
