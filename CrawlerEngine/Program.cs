using CrawlerEngine.Manager;
using System;

namespace CrawlerEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            new WorkManager().Process();
            Console.ReadLine();
        }
    }
}
