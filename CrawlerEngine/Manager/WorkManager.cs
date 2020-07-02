﻿using CrawlerEngine.Common.Helper;
using CrawlerEngine.Driver;
using CrawlerEngine.JobWorker;
using CrawlerEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrawlerEngine.Manager
{
    class WorkManager
    {
        private Condition resourseSetting;
        private List<string> mailTo;
        public void Process()
        {
            WebDriverPool.InitDriver(2);
            try
            {

                Parallel.ForEach(GetJobInfo(), jobInfo =>
                {
                    DoJob(jobInfo);
                });
            }
            catch (Exception e)
            {
                SendErrorEmail();
            }
            //
            // throw new Exception("沒做");
        }


        #region 工作區
        private IEnumerable<JobInfo> GetJobInfo()
        {
            return

              from x in Repository.Factory.CrawlFactory.CrawlDataJobListRepository.GetCrawlDataJobListDtos()
              select new JobInfo()
              {
                  Info = JsonUntityHelper.DeserializeStringToDictionary<string, object>(x.JobInfo),
                  Seq = x.Seq
              };
        }
        private List<JobInfo> Init()
        {

            throw new Exception("沒做");
        }
        private bool DoJob(JobInfo jobInfo)
        {
            var success = false;
            try
            {
                new JobWorkerFactory().GetJobWorker(jobInfo).DoJobFlow();
                success = true;
            }
            catch (Exception)
            {

            }
            return success;

        }
        private bool SendErrorEmail()
        {
            foreach (var user in mailTo)
            {

                //send error mail

            }
            throw new Exception("沒做");

        }



        #endregion
    }
}
