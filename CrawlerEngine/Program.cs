using CrawlerEngine.Manager;
using Npgsql;
using System;
using System.Net;

namespace CrawlerEngine
{
    class Program
    {
        private static int Resource = 1;


        private static string Host = "52.183.94.91";
        private static string User = "tomting";
        private static string DBname = "postgres";
        private static string Password = "!QAZ2wsx1234";
        private static string Port = "5432";


        static void Main(string[] args)
        {
            Guid a = Guid.NewGuid();
            a.ToString();
            // Build connection string using parameters from portal
            //
            string connString =
                String.Format(
                    "Server={0};Username={1};Database={2};Port={3};Password={4};SSLMode=Prefer",
                    Host,
                    User,
                    DBname,
                    Port,
                    Password);


            using (var conn = new NpgsqlConnection(connString))

            {
                Console.Out.WriteLine("Opening connection");
                conn.Open();
                
                using (var command = new NpgsqlCommand("INSERT INTO public.\"CrawlDataJobList\" (\"Seq\", \"JobInfo\",\"JobType\") VALUES (@Seq1, @JobInfo1,@JobType1), (@Seq2, @JobInfo2,@JobType2)", conn))
                {
                    command.Parameters.AddWithValue("@Seq1", Guid.NewGuid());
                    command.Parameters.AddWithValue("@JobInfo1", "JobInfo1");
                    command.Parameters.AddWithValue("@JobType1", "orange");
                    command.Parameters.AddWithValue("@Seq2", Guid.NewGuid());
                    command.Parameters.AddWithValue("@JobInfo2", "JobInfo2");
                    command.Parameters.AddWithValue("@JobType2", "orange");

                    int nRows = command.ExecuteNonQuery();
                    Console.Out.WriteLine(String.Format("Number of rows inserted={0}", nRows));
                }
            }

            Console.WriteLine("Press RETURN to exit");
            Console.ReadLine();

            //Console.WriteLine("To enable your free eval account and get CUSTOMER, "
            //    + "YOURZONE and YOURPASS, please contact sales@luminati.io");
            var client = new WebClient();
            // client.Proxy = new WebProxy("64.4.94.129:80");
            //client.Proxy.Credentials = new NetworkCredential("lum-customer-hl_a94e2c87-zone-static", "kkp5wtj7scek");
            try
            {
                Console.WriteLine(client.DownloadString("https://www.google.com/"));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //  LoggerHelper._.Error(ex: new System.Exception());
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
