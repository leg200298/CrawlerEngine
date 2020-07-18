using CrawlerEngine.Crawler.Interface;
using CrawlerEngine.Models;
using RestSharp;
using System;
using System.Net.Http;

namespace CrawlerEngine.Crawler.WorkClass
{
    public class HttpCrawler : ICrawler
    {
        private JobInfo jobInfo;

        public HttpCrawler(JobInfo jobInfo)
        {
            this.jobInfo = jobInfo;

        }

        public string DoCrawlerFlow()
        {
            return GetData();

        }

        private string GetData()
        {
            //var client = new RestClient(jobInfo.Url);
            //client.Timeout = -1;
            //var request = new RestRequest(Method.GET);
            //IRestResponse response = client.Execute(request);
            //return response.Content;
            var httpClient = new HttpClient();
            foreach (var key in jobInfo.HeaderDic.Keys)
            {
                httpClient.DefaultRequestHeaders.Add(key, jobInfo.HeaderDic[key]);
            }

            httpClient.Timeout = TimeSpan.FromSeconds(5);

            //httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/71.0.3578.98 Safari/537.36");
            //httpClient.DefaultRequestHeaders.Add("Referer", "https://ecshweb.pchome.com.tw/search/v3.3/");
            var httpResponse = httpClient.GetAsync(jobInfo.Url).Result;
            return httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        }


    }
}
