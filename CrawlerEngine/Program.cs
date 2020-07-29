using CrawlerEngine.Manager;
using System;

namespace CrawlerEngine
{
    class Program
    {
        private static int Resource = 1;

        static void Main(string[] args)
        {
            Guid a = Guid.NewGuid();
            a.ToString();

            //using (var conn = new NpgsqlConnection(ConnectionString.PostgresConnectionString))

            //{
            //    Console.Out.WriteLine("Opening connection");
            //    conn.Open();

            //    using (var command = new NpgsqlCommand("INSERT INTO public.crawl_data_job_list (seq, job_info,job_type) VALUES (@Seq1, @JobInfo1,@JobType1), (@Seq2, @JobInfo2,@JobType2)", conn))
            //    {
            //        command.Parameters.AddWithValue("@Seq1", Guid.NewGuid());
            //        command.Parameters.AddWithValue("@JobInfo1", @"{""_jobType"": ""MOMOSHOP-LGRPCATEGORY"",""_url"": ""https://www.momoshop.com.tw/category/LgrpCategory.jsp?l_code=2904400000&FTOOTH=29&Area=tooth&mdiv=1099600000-bt_0_996_11-&ctype=B&btType=C"",  ""_saveDataTime"": ""2020/07/22 04:59:50""}");
            //        command.Parameters.AddWithValue("@JobType1", "MOMOSHOP-LGRPCATEGORY");
            //        command.Parameters.AddWithValue("@Seq2", Guid.NewGuid());
            //        command.Parameters.AddWithValue("@JobInfo2", @"{ ""_jobType"": ""PCHOME24H-SEARCH"", ""_url"": ""https://ecshweb.pchome.com.tw/search/v3.3/?q=%E5%95%86%E5%93%81""}");
            //        command.Parameters.AddWithValue("@JobType2", "PCHOME24H-SEARCH");

            //        int nRows = command.ExecuteNonQuery();
            //        Console.Out.WriteLine(String.Format("Number of rows inserted={0}", nRows));
            //    }
            //}

            //Console.WriteLine("Press RETURN to exit");
            //Console.ReadLine();

            ////Console.WriteLine("To enable your free eval account and get CUSTOMER, "
            ////    + "YOURZONE and YOURPASS, please contact sales@luminati.io");
            //var client = new WebClient();
            //// client.Proxy = new WebProxy("64.4.94.129:80");
            ////client.Proxy.Credentials = new NetworkCredential("lum-customer-hl_a94e2c87-zone-static", "kkp5wtj7scek");
            //try
            //{
            //    Console.WriteLine(client.DownloadString("https://www.google.com/"));

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            ////  LoggerHelper._.Error(ex: new System.Exception());
            check(args);

            WorkManager workManager = new WorkManager();
            workManager.Process(Resource);
            //foreach (var data in Repository.Factory.CrawlFactory.CrawlDataJobListRepository.GetCrawlDataJobListDtos(2))
            //{

            //    Console.WriteLine(data.JobInfo);
            //}

            //new ChromeDriver();
            //Console.WriteLine("get gegegege");
            // Console.ReadLine();
        }
        static void check(string[] args)
        {

#if (!DEBUG)
            if (args.Count() == 0)
            {
                Console.WriteLine(" use default setting ");
               return ;
            }
            for (int i = 0; i < args.Count(); ++i)
            {
                if (args[i].Trim().ToLower() == "-h")
                {
                    Console.WriteLine(@"
-h | Help 
-r | Resource Count Setting");
                }
                if (args[i].Trim().ToLower() == "-r")
                {
                    try
                    {
                        Resource = Convert.ToInt32(args[i + 1]);
                    }
                    catch (Exception ex)
                    {
                        LoggerHelper._.Error(ex, "CommondError");
                        Console.WriteLine("ResourceSettingError use default : 1");
                        Resource = 1;

                    }
                }
            }
#endif
        }
    }
}
