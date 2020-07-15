using CrawlerEngine.Manager;

namespace CrawlerEngine
{
    class Program
    {
        private static int Resource = 1;
        static void Main(string[] args)
        {
            //// 1. 建立依賴注入的容器
            //var serviceCollection = new ServiceCollection();
            //// 2. 註冊服務
            //serviceCollection.AddTransient<IJobWorker, YahooMallProductJobWorker>();
            //// 建立依賴服務提供者
            //var serviceProvider = serviceCollection.BuildServiceProvider();

            //// 3. 執行主服務
            //serviceProvider.GetRequiredService<YahooMallProductJobWorker>().DoJobFlow();

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
                    catch (Exception)
                    {
                        LoggerHelper._.Error("CommondError", ex);
                        Console.WriteLine("ResourceSettingError use default : 1");
                        Resource = 1;

                    }
                }
            }
#endif
        }
    }
}
