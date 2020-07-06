using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CrawlerEngine.Repository.Common.Interface
{
    public interface IDatabaseConnectionHelper
    {
        IDbConnection Create();
    }
}
