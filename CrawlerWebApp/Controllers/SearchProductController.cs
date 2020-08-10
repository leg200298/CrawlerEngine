using CrawlerEngine.Common.Extansion;
using CrawlerEngine.Models;
using CrawlerEngine.Repository.Factory;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using static CrawlerEngine.Common.Enums.ElectronicBusiness;

namespace CrawlerWebApp.Controllers
{
    public class SearchProductController : Controller
    {
        public SearchProductController()
        {
            crawlFactory = new CrawlFactory("POSTGRESSQL");
        }


        private CrawlFactory crawlFactory;
        // GET: SearchProduct
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetFeeBeeData([FromBody] FeeBeeRequest data)
        {
            return Json(GetDataFromFeebee(data.product));
            //var response = crawlFactory.CrawlDataJobListRepository.GetCrawlDataJobDiffStatus();
            //return Json(response);
        }
        [HttpPost]
        public IActionResult sendFeeBeeDataToJob([FromBody] FeeBeeStruct data)
        {
            try
            {
                JobInfo jobInfo = new JobInfo();
                jobInfo.Url = data.href;
                jobInfo.JobType = GetJobType(data);
                jobInfo.PutToDic("_title", data.title);
                jobInfo.PutToDic("_price", data.data_price);
                if (string.IsNullOrEmpty(jobInfo.JobType)) return Json(new ReturnMessage<object>
                {
                    success = false,
                    returnMsg = "No Provider"
                });
                crawlFactory.CrawlDataJobListRepository.InsertOne(jobInfo, jobInfo.JobType);
                return Json(new ReturnMessage<object>
                {
                    success = true,
                    returnMsg = data.title + "  新增成功"
                });
            }
            catch (Exception ex)
            {

                return Json(new ReturnMessage<object>
                {
                    success = false,
                    returnMsg = ex.Message + ex.StackTrace
                });
            }


        }


        [HttpPost]
        public IActionResult sendAllFeeBeeDataToJob([FromBody] List<SelectFeeBeeStruct> datas)
        {
            try
            {

                JobInfo jobInfo = new JobInfo();
                foreach (var data in datas.Where(x => x.selected))
                {
                    jobInfo.Url = data.href;
                    jobInfo.JobType = GetJobType(data);
                    jobInfo.PutToDic("_title", data.title);
                    jobInfo.PutToDic("_price", data.data_price);
                    if (string.IsNullOrEmpty(jobInfo.JobType)) return Json(new ReturnMessage<object>
                    {
                        success = false,
                        returnMsg = "No Provider"
                    });
                    crawlFactory.CrawlDataJobListRepository.InsertOne(jobInfo, jobInfo.JobType);
                }
                return Json(new ReturnMessage<object>
                {
                    success = true,
                    returnMsg = " 新增成功"
                });
            }
            catch (Exception ex)
            {

                return Json(new ReturnMessage<object>
                {
                    success = false,
                    returnMsg = ex.Message + ex.StackTrace
                });
            }


        }

        private List<FeeBeeStruct> GetDataFromFeebee(string product)
        {
            var httpClient = new HttpClient();
            var targetUrl = $"https://feebee.com.tw/s/?q={product}";
            var httpResponse = httpClient.GetAsync(targetUrl).GetAwaiter().GetResult();


            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult());
            var cc = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"list_view\"]/li/span[2]/a");
            List<FeeBeeStruct> la = new List<FeeBeeStruct>();
            FeeBeeStruct a = new FeeBeeStruct();
            foreach (var data in cc)
            {
                a = new FeeBeeStruct();
                a.href = data.GetAttributes("href").FirstOrDefault().Value.Replace("&amp;", "&");
                a.data_store = data.GetAttributes("data-store").FirstOrDefault()?.Value;
                a.data_price = data.GetAttributes("data-price").FirstOrDefault().Value;
                a.data_provider = data.GetAttributes("data-provider").FirstOrDefault().Value;
                a.title = data.GetAttributes("title").FirstOrDefault().Value;

                la.Add(a);
            }
            return la;
            // File.WriteAllText("testfeebee.txt", JsonConvert.SerializeObject(la));
        }
        private string GetJobType(FeeBeeStruct data)
        {
            string returnStr = string.Empty;
            if (data.data_provider.ToLower() == "momoshop")
            {
                returnStr = Platform.MomoShopProduct.GetDescription();
            }
            if (data.data_provider.ToLower() == "24hpchome")
            {
                returnStr = Platform.Pchome24hProduct.GetDescription();
            }
            if (data.data_provider.ToLower() == "ybuy")
            {
            }
            if (data.data_provider.ToLower() == "jollybuy")
            {
            }
            if (data.data_provider.ToLower() == "etmall")
            {
            }
            if (data.data_provider.ToLower() == "umall")
            {
            }
            if (data.data_provider.ToLower() == "spstore")
            {
            }
            
            return returnStr;
        }
        public class ReturnMessage<T>
        {
            public bool success { get; set; }
            public string returnMsg { get; set; }
            public T data { get; set; }
        }
        public class FeeBeeRequest
        {
            public string product { get; set; }
        }
        public class FeeBeeStruct
        {
            public string href { get; set; }
            public string data_store { get; set; }
            public string data_price { get; set; }
            public string data_provider { get; set; }
            public string title { get; set; }
        }
        public class SelectFeeBeeStruct : FeeBeeStruct
        {

            public bool selected { get; set; }
        }


    }
}