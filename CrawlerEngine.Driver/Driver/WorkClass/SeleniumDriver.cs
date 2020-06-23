using CrawlerEngine.Driver.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrawlerEngine.Driver.WorkClass
{
    public class SeleniumDriver : IDriver
    {
        public string Status { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Init()
        {
            throw new Exception();
            
        }
    }
}
