using CrawlerEngine.Manager;
using System;

namespace CrawlerEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            WorkManager workManager = new WorkManager();
            workManager.Process();
            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }
    }
}
