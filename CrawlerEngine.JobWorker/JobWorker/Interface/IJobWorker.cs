namespace CrawlerEngine.JobWorker.Interface
{
    public interface IJobWorker
    {
        /// <summary>
        ///  執行工作流程
        /// </summary>
        void DoJobFlow();
    }
}
