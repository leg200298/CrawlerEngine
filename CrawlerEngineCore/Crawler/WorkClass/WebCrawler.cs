﻿using CrawlerEngine.Common.Helper;
using CrawlerEngine.Crawler.Interface;
using CrawlerEngine.Driver;
using CrawlerEngine.Models;
using System;

namespace CrawlerEngine.Crawler.WorkClass
{
    public class WebCrawler : ICrawler
    {
        private JobInfo jobInfo;

        private int driverId;
        public WebCrawler(JobInfo jobInfo)
        {
            this.jobInfo = jobInfo;
        }

        public string DoCrawlerFlow()
        {
            try
            {
                GetDriver();
                OpenUrl();
                return GetData();
            }
            catch (Exception ex)
            {

                LoggerHelper._.Error(ex);
                return null;
            }
        }

        protected string GetData()
        {
            string responseData = string.Empty;
            try
            {
                //WebDriverPool.DriverPool[driverId].ChromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                ////var wait = new WebDriverWait(WebDriverPool.DriverPool[driverId].ChromeDriver, TimeSpan.FromSeconds(10));
                ////var t = wait.Until<string>(WebDriverPool.DriverPool[driverId].ChromeDriver.FindElementByXPath("/html/body").GetAttribute("innerHTML"));
                //Thread.Sleep(10);
                responseData = WebDriverPool.DriverPool[driverId].ChromeDriver.FindElementByXPath("/html/body").GetAttribute("innerHTML");
            }
            catch (Exception ex)
            {

                LoggerHelper._.Error(ex);
            }
            finally
            {
                WebDriverPool.DriverPool[driverId].Status = Common.Enums.ObjectStatus.Driver.FREE;
            }
            return responseData;
        }

        private void OpenUrl()
        {
            WebDriverPool.DriverPool[driverId].ChromeDriver.Navigate().GoToUrl(jobInfo.Url);
        }


        private void GetDriver()
        {

            driverId = WebDriverPool.GetFreeDriver();

            WebDriverPool.DriverPool[driverId].Status = Common.Enums.ObjectStatus.Driver.NOTFREE;

        }

    }
}
