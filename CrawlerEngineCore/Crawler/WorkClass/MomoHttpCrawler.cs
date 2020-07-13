using CrawlerEngine.Crawler.Interface;
using CrawlerEngine.Models;
using System.Net.Http;
using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrawlerEngine.Crawler.WorkClass
{
    public class MomoHttpCrawler : ICrawler
    {
        private JobInfo jobInfo;      

        public MomoHttpCrawler(JobInfo jobInfo)
        {            
            this.jobInfo = jobInfo;
            
        }

        public string DoCrawlerFlow()
        {
            return GetData();
        }

        private string GetData()
        {
            Uri uri = new Uri(jobInfo.Url);
            HttpClientHandler handler = new HttpClientHandler() {
                CookieContainer = new CookieContainer()                
            };                        
            var httpClient = new HttpClient(handler);
            httpClient.GetAsync("https://" + uri.Host).GetAwaiter().GetResult();   
            
            foreach (var key in jobInfo.HeaderDic.Keys)
            {
                httpClient.DefaultRequestHeaders.Add(key, jobInfo.HeaderDic[key]);
            }
            var postData = new StringContent(Convert.ToString(jobInfo.GetFromDic("_postData"))
                , Encoding.UTF8, "application/x-www-form-urlencoded");
            string url = jobInfo.Url + "&t=" 
                + ((Int64)new TimeSpan(DateTime.UtcNow.Ticks - new DateTime(1970, 1, 1).Ticks)
                               .TotalMilliseconds).ToString();

            var httpResponse = httpClient.PostAsync(url, postData).GetAwaiter().GetResult();
            return httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        }
    }
}
