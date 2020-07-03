using CrawlerEngine.Manager;

namespace CrawlerEngine
{
    class Program
    {
        static void Main(string[] args)
        {

            WorkManager workManager = new WorkManager();
            workManager.Process();
        }
    }
}
