using CrawlerEngine.Common.Helper;
using CrawlerEngine.Driver;
using CrawlerEngine.Models;
using CrawlerEngine.Repository.Factory;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace CrawlerEngine.JobWorker.WorkClass
{
    /// <summary>
    /// 商品細節頁
    /// </summary>
    public class PttPageJobWorker : JobWorkerBase
    {
        public override Logger _logger { get => LogManager.GetCurrentClassLogger(); }
        private int page = 1;
        private HtmlDocument htmlDoc = new HtmlDocument();
        private List<PttData> pttDatas = new List<PttData>();
        public PttPageJobWorker(JobInfo jobInfo)
        {
            this.jobInfo = jobInfo;
            page = Convert.ToInt32(jobInfo.GetFromDic("_pageStart"));
        }
        JObject t = new JObject();
        public override JobInfo jobInfo { get; set; }
        private int driverId;
        protected override bool Crawl()
        {
            var targetUrl = $"https://www.ptt.cc/bbs/{jobInfo.GetFromDic("_board")}/index{page}.html";
            var success = false;
            try
            {

                var httpClient = new HttpClient();
                var httpResponse = httpClient.GetAsync(targetUrl).GetAwaiter().GetResult();

                responseData = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                //GetDriver();
                //OpenUrl();
                //responseData = GetData();
                success = true;
            }
            catch (Exception ex)
            {
                LoggerHelper._.Error(ex);
            }
            return success;
        }

        private HttpClient SetHttpHeader(HttpClient httpClient)
        {
            //httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.105 Safari/537.36");
            //httpClient.DefaultRequestHeaders.Add("Referer", jobInfo.Url);

            httpClient.DefaultRequestHeaders.Add("User-Agent", "Googlebot");
            httpClient.DefaultRequestHeaders.Add("Referer", "https://www.google.com");

            return httpClient;
        }

        protected override bool GotoNextPage(string url)
        {
            return false;
        }

        protected override (bool, string) HasNextPage()
        {
            var pageEnd = Convert.ToInt32(jobInfo.GetFromDic("_pageEnd"));
            if (page < pageEnd)
            {
                page++;
                return (true, "");
            }
            return (false, "");
        }

        protected override bool Parse()
        {
            htmlDoc.LoadHtml(responseData);
            HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"main-container\"]/div[2]/div");
            PttData pttData = new PttData();

            //  Parallel.ForEach(nodes, data =>
            foreach (HtmlNode data in nodes)
            {
                pttData = new PttData();
                try
                {
                    var t = 0;
                    int.TryParse(data.SelectSingleNode("div[1]")?.InnerText, out t);
                    pttData.nrec = t;
                    pttData.title = data.SelectSingleNode("div[2]/a")?.InnerText;
                    pttData.url = data.SelectSingleNode("div[2]/a") == null ? null : "https://www.ptt.cc" + data.SelectSingleNode("div[2]/a").Attributes["href"].Value;
                    pttData.author = data.SelectSingleNode("div[3]/div[1]")?.InnerText;
                    pttData.mark = data.SelectSingleNode("div[3]/div[4]")?.InnerText;
                    pttData.date = data.SelectSingleNode("div[3]/div[3]")?.InnerText;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                if (!string.IsNullOrWhiteSpace(pttData.url))
                {
                    pttData.id = pttData.url.Split('/').LastOrDefault().Replace(".html", "");

                    var httpResponse = new HttpClient().GetAsync($"https://www.ptt.cc/bbs/{jobInfo.GetFromDic("_board")}/{pttData.id}.html").GetAwaiter().GetResult();

                    var responseinnerData = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    if (responseinnerData.StartsWith("404 page not found")) return false;
                    var qqqq = "";
                    StringBuilder sb = new StringBuilder();
                    try
                    {
                        HtmlDocument innerHtmlDoc = new HtmlDocument();
                        innerHtmlDoc.LoadHtml(responseinnerData);
                        pttData.date = innerHtmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"main-content\"]/div[4]/span[2]").InnerText;
                        var t2 = innerHtmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"main-content\"]");
                        qqqq = t2.InnerText;
                        foreach (var t in t2.SelectNodes("//*[@id=\"main-content\"]/div").Where(x => !string.IsNullOrEmpty(x.InnerText)))
                        {
                            try
                            {
                                qqqq = qqqq.Replace(t.InnerText, "");
                                sb.AppendLine(t.InnerText);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }

                        }
                    }
                    catch (Exception)
                    {


                    }

                    pttData.content = qqqq;

                    pttData.contentOther = sb;

                    pttDatas.Add(pttData);
                }
                //pttData.nrec = htmlDoc.DocumentNode.SelectSingleNode

            };
            //*[@id="main-container"]/div[2]/div[15]/div[1]
            //*[@id="main-container"]/div[2]/div[16]/div[1]
            //*[@id="main-container"]/div[2]/div[16]/div[2]
            return true;

        }

        protected override bool SaveData(CrawlFactory crawlFactory)
        {
            var RootPath = $"Ptt/{jobInfo.GetFromDic("_board")}";
            var TitlePath = RootPath + "/title/";
            var AuthorPath = RootPath + "/author/";
            var NrecPath = RootPath + "/nrec/";
            var ContentPath = RootPath + "/content/";
            var DatePath = RootPath + "/date/";
            var ContentOtherPath = RootPath + "/contentOther/";
            CheckDic(RootPath);
            CheckDic(TitlePath);
            CheckDic(AuthorPath);
            CheckDic(ContentPath);
            CheckDic(NrecPath);
            CheckDic(DatePath);
            CheckDic(ContentOtherPath);

            foreach (var data in pttDatas)
            {
                File.AppendAllText($"{TitlePath}{data.id}.txt", data.title);
                File.AppendAllText($"{AuthorPath}{data.id}.txt", data.author);
                File.AppendAllText($"{NrecPath}{data.id}.txt", data.nrec.ToString());
                File.AppendAllText($"{ContentPath}{data.id}.txt", data.content);
                File.AppendAllText($"{DatePath}{data.id}.txt", data.date);
                File.AppendAllText($"{ContentOtherPath}{data.id}.txt", data.contentOther.ToString());
                //   File.AppendAllText($"{directoryPath}/Url/" + data.id + ".txt", data.url);
            }

            pttDatas.Clear();
            return true;

        }

        private static void CheckDic(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        protected override void SleepForAWhile(decimal sleepTime)
        {

        }


        protected override bool Validate()
        {
            if (string.IsNullOrEmpty(responseData))
            {
                return false;
            }
            else
            {
                try
                {
                    //try
                    //{
                    //    t = JObject.Parse(responseData);
                    //}
                    //catch { throw new Exception($"Parse Error {responseData}"); }

                    //if (t["status"].ToString().ToUpper() != "OK") throw new Exception("Api Error");
                    //if (t["data"] == null) throw new Exception("No Data");
                    //if (t["data"].ToString().ToLower() == "stock code not exist") throw new Exception("Code Not Exist");
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return true;
            }
        }

        class PttData
        {
            public string url { get; set; }
            public string date { get; set; }
            public int nrec { get; set; }
            public string author { get; set; }
            public string mark { get; set; }
            public string title { get; set; }
            public string id { get; set; }
            public string content { get; set; }
            public StringBuilder contentOther { get; set; }

        }
        #region WebBrowser

        private void GetDriver()
        {

            driverId = WebDriverPool.GetFreeDriver();

            WebDriverPool.DriverPool[driverId].Status = Common.Enums.ObjectStatus.Driver.NOTFREE;

        }
        private void OpenUrl()
        {
            WebDriverPool.DriverPool[driverId].ChromeDriver.Navigate().GoToUrl(jobInfo.Url);
        }



        protected string GetData()
        {
            string responseData = string.Empty;
            try
            {
                WebDriverPool.DriverPool[driverId].ChromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                responseData = WebDriverPool.DriverPool[driverId].ChromeDriver.FindElementByXPath("/html/body").GetAttribute("innerHTML");
                ScrollMove();

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


        private void ScrollMove()
        {
            OpenQA.Selenium.IJavaScriptExecutor jse = WebDriverPool.DriverPool[driverId].ChromeDriver;
            int height = (int)Math.Ceiling(1000 * 0.1);
            jse.ExecuteScript("window.scrollBy(0," + height + ")");
        }

        #endregion

    }
}
