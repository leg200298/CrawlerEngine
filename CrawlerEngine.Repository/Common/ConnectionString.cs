﻿namespace CrawlerEngine.Repository.Common
{
    public static class ConnectionString
    {
        public static string SensenConnectionString =>
             "Data Source=dalab01.etzone.net,1437;Initial Catalog=MLDL;Persist Security Info=True;User ID=Formal_User;Password=1qaz2wsx!@;";
        public static string AzureConnectionString =>
             "Data Source=tomting.database.windows.net,1433;Initial Catalog=BookStore;Persist Security Info=True;User ID=tomting;Password=!QAZ2wsx;";
        public static string PostgresConnectionString =>
               "Server=52.148.175.204;Username=tomting;Database=postgres;Port=5432;Password=!QAZ2wsx1234;SSLMode=Prefer;Timeout=0;";


        //   "Data Source=dalab01.etzone.net,1437;Initial Catalog=MLDL;Persist Security Info=True;User ID=Formal_User;Password=1qaz2wsx!@;connection reset=false;connection lifetime=50;min pool size=1;max pool size=50";
        //ConfigurationManager.ConnectionStrings["SensenConnection"].ConnectionString;

    }
}
