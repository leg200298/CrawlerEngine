using CrawlerEngine.Manager;
using System;
using System.Linq;

namespace CrawlerEngine
{
    class Program
    {
        private static int Resource = 1;
        private static int Browser = 1;

        static void Main(string[] args)
        {
            
            check(args);

            WorkManager workManager = new WorkManager();
            workManager.Process(Resource, Browser);
        }
        static void check(string[] args)
        {

#if (!DEBUG)
            if (args.Count() == 0)
            {
                Console.WriteLine(" use default setting ");
                return;
            }
            for (int i = 0; i < args.Count(); ++i)
            {
                if (args[i].Trim().ToLower() == "-h")
                {
                    Console.WriteLine(@"
-h | Help 
-r | Resource Count Setting
-w | Resource Count Setting");
                }
                if (args[i].Trim().ToLower() == "-r")
                {
                    try
                    {
                        Resource = Convert.ToInt32(args[i + 1]);
                    }
                    catch (Exception ex)
                    {
                        //LoggerHelper._.Error(ex, "CommondError");
                        Console.WriteLine("ResourceSettingError use default : 1");
                        Resource = 1;

                    }
                }
                if (args[i].Trim().ToLower() == "-w")
                {
                    try
                    {
                        Browser = Convert.ToInt32(args[i + 1]);
                    }
                    catch (Exception ex)
                    {
                        //LoggerHelper._.Error(ex, "CommondError");
                        Console.WriteLine("ResourceSettingError use default : 1");
                        Browser = 1;

                    }
                }
            }
#endif
        }
    }
}
