using CrawlerEngine.Common.Helper;
using CrawlerEngine.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;

namespace CrawlerEngine.JobWorker.WorkClass
{

    /// <summary>
    /// 館分類頁
    /// </summary>
    class RedMineJobWorker : JobWorkerBase
    {
        private List<JobInfo> jobInfos = new List<JobInfo>();
        private HtmlDocument htmlDoc = new HtmlDocument();
        private string innerData = string.Empty;

        public RedMineJobWorker(JobInfo jobInfo)
        {


            this.jobInfo = jobInfo;

        }
        public override JobInfo jobInfo { get; set; }

        //private ResponseObject tempResponseObject = new ResponseObject();

        protected override bool GotoNextPage(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }
            else
            {

                jobInfo.Url = url;

                return true;
            }
        }

        protected override bool Crawl()
        {
            var success = false;
            try
            {
                var httpClient = new HttpClient();
                SetHttpHeader(httpClient);
                var httpResponse = httpClient.GetAsync(jobInfo.Url).GetAwaiter().GetResult();

                responseData = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                success = true;
            }
            catch (Exception ex)
            {
                LoggerHelper._.Error(ex);
            }
            return success;
        }

        private static void SetHttpHeader(HttpClient httpClient)
        {
            // httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/71.0.3578.98 Safari/537.36");
            httpClient.DefaultRequestHeaders.Add("Cookie", "_ga=GA1.2.264222352.1578902403; _atrk_siteuid=NsqyNNwHatycJ-WK; dcs_local_cid=0njgw7t2jx; _redmine_session=K0xhSTRBRGc5eXhRazN2bXd6UjBzZFZaWXpvMUxIQWxuSE43cXgrbHp2WUM2NHFvMkVoL3VoNWRVTmJlbC9mWmhkNXlacEFBYTRJS0pMNHhhRUx0MFFaSFhocWdzNGMxOTZRSXY0QTBYUS9FYXFrY2JPUWdkTXlUbmtRNUQwOFpWcDhqV2dONlJHN2FUSTh5a292dVk3SFRZMGhscTkwb0FsdFRnUld6MVNxRE1Qekh1QlY2ZEpWYVJiUENrYncrdWN1SDlsUmg4ZWFIb0hSRE1pWVFkNnQxZjlkbHRwNzUzcEgyQktEeE0xcFRkckdSeFJFaFMzK1kvQmQzRDhwanlzUXpTa0VmYmlGVnpuaHB4NWxkVmc9PS0tUXMrN05qU2pEVjVCWUtQMHE3dTZYUT09--ac857e023d1b4a7a665e4d21844d70f6c1cff2f8; _redmine_session=bUdjenBzei9Ieis4Nk9Jb0ZWQUJPaitBa2N2QUNxOHkrYmg2SndpUnZQZXA0NzlveVh4N2U1Y09SR3FiRGVMZDlKazIvOHgvb0tYT09DdE4vWW01bXJtYWZDYytWVzhRdWZLQWNvUng0OXBtWHU2MHh3YXhzWUhCZHpkWjFwTWh6cnpIZUxIcVVjTklVaVJyRUhWVGZDaW5peTZ4UmZTeVBKZVEyNjZDNHdEVm1OM2MzeEorQ3djOFRnWlRsT1ROQlJkSnRTdmdNUzRIWC81ZUllT0ViaTFUR1RQVHV1T2pTUzV0OVZIT3Z6a2FxRU5pWnhzeGtmNVVZQUJyYmRvTHpaVm45c0s2WVNMYmQrMXVQZ1l2bVE9PS0tcFE2T3B6VnJmZ2VMMmk5bXhqQ1kwZz09--df8fa777ebb4b88d031c7a5dbe99444889b80a81");
            //httpClient.DefaultRequestHeaders.Add("Referer", "https://ecshweb.pchome.com.tw/search/v3.3/");
        }

        protected override bool Validate()
        {
            if (string.IsNullOrEmpty(responseData))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        protected override bool Parse()
        {

            try
            {
                htmlDoc.LoadHtml(responseData);

                innerData = htmlDoc.DocumentNode.SelectSingleNode(" //*[@id=\"content\"]").InnerText;
                // innerData = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"content\"]/div[2]/div[3]").InnerText;

                try
                {
                    innerData = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"content\"]/div[2]/div[3]").InnerText;
                }
                catch { }

                return true;
            }
            catch
            {
                return false;
            }
        }

        protected override bool SaveData()
        {
            File.AppendAllText("Redmine/" + jobInfo.Url.Split('/').LastOrDefault() + ".txt", innerData);

            return true;

        }

        protected override (bool, string) HasNextPage()
        {
            return (false, "");
        }

        protected override void SleepForAWhile(decimal sleepTime)
        {
            Thread.Sleep((int)(sleepTime * 1000));
        }





    }
}
