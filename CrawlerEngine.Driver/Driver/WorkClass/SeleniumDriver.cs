using CrawlerEngine.Driver.Interface;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrawlerEngine.Driver.WorkClass
{
    public class SeleniumDriver : ChromeDriver
    {
        public string Status { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public SeleniumDriver Init()
        {
           return this;
        }

    }
}
