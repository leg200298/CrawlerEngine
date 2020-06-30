using CrawlerEngine.Manager;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace CrawlerEngine
{
    class Program
    {
        static void Main(string[] args)
        {
           // var t = Repository.Factory.CrawlFactory.CrawlDataJobListRepository.GetCrawlDataJobListDtos();
           //var info =  JObject.Parse(t.FirstOrDefault().JobInfo);
           // Console.WriteLine(info["url"]);
           // Console.WriteLine(info["status"]);
           // Console.WriteLine(info["xPathList"]);
            WorkManager workManager = new WorkManager();
            workManager.Process();
            Console.ReadLine();
        }
    }
}
