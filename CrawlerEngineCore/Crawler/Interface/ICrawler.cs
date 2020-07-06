namespace CrawlerEngine.Crawler.Interface
{
    public interface ICrawler
    {

        /// <summary>
        /// 執行爬蟲流程
        /// </summary>
        /// <returns></returns>
        string DoCrawlerFlow();
    }
}
