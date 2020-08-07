using CrawlerEngine.Common.Helper;
using CrawlerEngine.JobWorker.Interface;
using CrawlerEngine.Models;
using CrawlerEngine.Models.Models;
using CrawlerEngine.Repository.Factory;
using CrawlerEngine.Repository.Interface;
using CrawlerEngine.SignalR;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrawlerEngine.JobWorker
{
    public abstract class JobWorkerBase : IJobWorker
    {
        public abstract Logger _logger { get; }

        public ICrawlDataJobListRepository crawlDataJobListRepository;
        public abstract JobInfo jobInfo { get; set; }
        public string responseData;

        private decimal sleepTime = 0;
        protected JsonOptions crawlDataDetailOptions = new JsonOptions();
        protected List<JsonOptions> lCrawlDataDetailOptions = new List<JsonOptions>();

        /// <summary>
        /// 執行工作流程 ()
        /// </summary>
        public void DoJobFlow(CrawlFactory crawlFactory)
        {
            this.crawlDataJobListRepository = crawlFactory.CrawlDataJobListRepository;           
            UpdateJobStatusStart();
            Task.Run(async () => await new JobHub().SendJobInfo(jobInfo.Seq, jobInfo.JobType, jobInfo.Url));
            (bool, string) temp = (false, "");
            try
            {
                //  _logger.Info("tTESTDATA");
                _logger.Info($"{jobInfo.Seq}  Start");
                do
                {
                    GotoNextPage(temp.Item2);
                    Crawl();

                    _logger.Info($"{jobInfo.Seq}  GetData");
                    if (Validate())
                    {
                        _logger.Info($"{jobInfo.Seq}  Validate");
                        if (Parse())
                        {
                            _logger.Info($"{jobInfo.Seq}  Parse");
                            SaveData(crawlFactory);
                            _logger.Info($"{jobInfo.Seq}  SaveData");
                        }
                        temp = HasNextPage();
                        SleepForAWhile(GetSleepTimeByJobInfo());
                    }
                } while (temp.Item1);
                UpdateJobStatusEnd();
                Task.Run(async () => await new JobHub().SendJobInfo(jobInfo.Seq, jobInfo.JobType, jobInfo.Url));
                _logger.Info($"{jobInfo.Seq}  End");
            }
            catch (Exception e)
            {


                if (e.Message.StartsWith("Parse Error"))
                {
                    _logger.Error(e, jobInfo.Seq.ToString() + responseData);
                    jobInfo.ErrorInfo = e.Message; UpdateJobStatusWaitForaWhile();
                }
                else
                {
                    _logger.Info(e, jobInfo.Seq.ToString() + "Parse Error");
                    UpdateJobStatusFail();
                }


            }

        }
        private void UpdateJobStatusStart()
        {
            crawlDataJobListRepository.UpdateStatusStart(jobInfo);

        }

        protected abstract bool Crawl();
        protected abstract bool Validate();
        protected abstract bool Parse();
        protected abstract bool SaveData(CrawlFactory crawlFactory);
        protected abstract (bool, string) HasNextPage();
        protected abstract bool GotoNextPage(string url);
        private decimal GetSleepTimeByJobInfo()
        {
            try
            {
                sleepTime = jobInfo.DriverSleepTime ??
                    2 + new Random().Next(3, 100) / 50;
            }
            catch (Exception ex)
            {

                LoggerHelper._.Error(ex);
            }

            return sleepTime;
        }
        protected abstract void SleepForAWhile(decimal sleepTime);
        private void UpdateJobStatusEnd()
        {
            crawlDataJobListRepository.UpdateStatusEnd(jobInfo);
        }
        private void UpdateJobStatusFail()
        {
            crawlDataJobListRepository.UpdateJobStatusFail(jobInfo);
        }
        private void UpdateJobStatusWaitForaWhile()
        {
            crawlDataJobListRepository.UpdateStatusWaitForaWhile(jobInfo);
        }
    }
}
